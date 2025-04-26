using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

public class ImGuiController
{
    private bool _frameBegun;
    private int _vertexArray;
    private int _vertexBuffer;
    private int _indexBuffer;
    private int _fontTexture;
    private int _shaderProgram;
    private int _attribLocationTex;
    private int _attribLocationProjMtx;
    private int _attribLocationVtxPos;
    private int _attribLocationVtxUV;
    private int _attribLocationVtxColor;

    public ImGuiController(int width, int height)
    {
        ImGui.CreateContext();
        ImGui.GetIO().Fonts.AddFontDefault();
        ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        ImGui.GetIO().DisplaySize = new System.Numerics.Vector2(width, height);

        CreateDeviceResources();
        SetPerFrameImGuiData(1f / 60f);
    }

    public void WindowResized(int width, int height)
    {
        ImGui.GetIO().DisplaySize = new System.Numerics.Vector2(width, height);
    }

    public void Update(GameWindow wnd, float deltaSeconds)
    {
        if (_frameBegun)
            ImGui.Render();

        SetPerFrameImGuiData(deltaSeconds);
        UpdateImGuiInput(wnd);

        ImGui.NewFrame();
        _frameBegun = true;
    }

    public void Render()
    {
        if (!_frameBegun)
            return;

        _frameBegun = false;
        ImGui.Render();
        RenderImDrawData(ImGui.GetDrawData());
    }

    private void SetPerFrameImGuiData(float deltaSeconds)
    {
        ImGuiIOPtr io = ImGui.GetIO();
        io.DeltaTime = deltaSeconds > 0.0f ? deltaSeconds : 1.0f / 60.0f;
    }

    private void UpdateImGuiInput(GameWindow wnd)
    {
        ImGuiIOPtr io = ImGui.GetIO();

        var mouse = wnd.MouseState;
        var keyboard = wnd.KeyboardState;

        io.MouseDown[0] = mouse.IsButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left);
        io.MouseDown[1] = mouse.IsButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Right);
        io.MouseDown[2] = mouse.IsButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle);
        io.MousePos = new System.Numerics.Vector2(mouse.X, mouse.Y);

        foreach (Keys key in Enum.GetValues(typeof(Keys)))
        {
            ImGuiKey imguiKey = ConvertToImGuiKey(key);
            if (imguiKey != ImGuiKey.None)
                io.AddKeyEvent(imguiKey, keyboard.IsKeyDown(key));
        }

        io.KeyCtrl = keyboard.IsKeyDown(Keys.LeftControl) || keyboard.IsKeyDown(Keys.RightControl);
        io.KeyAlt = keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt);
        io.KeyShift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
        io.KeySuper = keyboard.IsKeyDown(Keys.LeftSuper) || keyboard.IsKeyDown(Keys.RightSuper);
    }

    private ImGuiKey ConvertToImGuiKey(Keys key)
    {
        return key switch
        {
            Keys.Tab => ImGuiKey.Tab,
            Keys.Left => ImGuiKey.LeftArrow,
            Keys.Right => ImGuiKey.RightArrow,
            Keys.Up => ImGuiKey.UpArrow,
            Keys.Down => ImGuiKey.DownArrow,
            Keys.PageUp => ImGuiKey.PageUp,
            Keys.PageDown => ImGuiKey.PageDown,
            Keys.Home => ImGuiKey.Home,
            Keys.End => ImGuiKey.End,
            Keys.Delete => ImGuiKey.Delete,
            Keys.Backspace => ImGuiKey.Backspace,
            Keys.Enter => ImGuiKey.Enter,
            Keys.Escape => ImGuiKey.Escape,
            Keys.A => ImGuiKey.A,
            Keys.C => ImGuiKey.C,
            Keys.V => ImGuiKey.V,
            Keys.X => ImGuiKey.X,
            Keys.Y => ImGuiKey.Y,
            Keys.Z => ImGuiKey.Z,
            _ => ImGuiKey.None,
        };
    }

    private void CreateDeviceResources()
    {
        _vertexArray = GL.GenVertexArray();
        _vertexBuffer = GL.GenBuffer();
        _indexBuffer = GL.GenBuffer();

        RecreateFontDeviceTexture();

        const string vertexShaderSource = @"#version 330 core
uniform mat4 projection_matrix;
layout (location = 0) in vec2 in_position;
layout (location = 1) in vec2 in_texCoord;
layout (location = 2) in vec4 in_color;
out vec2 frag_UV;
out vec4 frag_Color;
void main()
{
    frag_UV = in_texCoord;
    frag_Color = in_color;
    gl_Position = projection_matrix * vec4(in_position, 0, 1);
}";

        const string fragmentShaderSource = @"#version 330 core
in vec2 frag_UV;
in vec4 frag_Color;
uniform sampler2D in_texture;
out vec4 out_Color;
void main()
{
    out_Color = frag_Color * texture(in_texture, frag_UV.st);
}";

        int vertex = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertex, vertexShaderSource);
        GL.CompileShader(vertex);

        int fragment = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragment, fragmentShaderSource);
        GL.CompileShader(fragment);

        _shaderProgram = GL.CreateProgram();
        GL.AttachShader(_shaderProgram, vertex);
        GL.AttachShader(_shaderProgram, fragment);
        GL.LinkProgram(_shaderProgram);

        GL.DetachShader(_shaderProgram, vertex);
        GL.DetachShader(_shaderProgram, fragment);
        GL.DeleteShader(vertex);
        GL.DeleteShader(fragment);

        _attribLocationTex = GL.GetUniformLocation(_shaderProgram, "in_texture");
        _attribLocationProjMtx = GL.GetUniformLocation(_shaderProgram, "projection_matrix");

        _attribLocationVtxPos = 0;
        _attribLocationVtxUV = 1;
        _attribLocationVtxColor = 2;

        GL.BindVertexArray(_vertexArray);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, 10000, IntPtr.Zero, BufferUsageHint.DynamicDraw);

        GL.EnableVertexAttribArray(_attribLocationVtxPos);
        GL.VertexAttribPointer(_attribLocationVtxPos, 2, VertexAttribPointerType.Float, false, 20, 0);
        GL.EnableVertexAttribArray(_attribLocationVtxUV);
        GL.VertexAttribPointer(_attribLocationVtxUV, 2, VertexAttribPointerType.Float, false, 20, 8);
        GL.EnableVertexAttribArray(_attribLocationVtxColor);
        GL.VertexAttribPointer(_attribLocationVtxColor, 4, VertexAttribPointerType.UnsignedByte, true, 20, 16);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    private void RecreateFontDeviceTexture()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out _);

        int prevTex;
        GL.GetInteger(GetPName.TextureBinding2D, out prevTex);

        _fontTexture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _fontTexture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                      PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        io.Fonts.SetTexID((IntPtr)_fontTexture);
        io.Fonts.ClearTexData();

        GL.BindTexture(TextureTarget.Texture2D, prevTex);
    }

    private void RenderImDrawData(ImDrawDataPtr drawData)
    {
        GL.Enable(EnableCap.Blend);
        GL.BlendEquation(BlendEquationMode.FuncAdd);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.Disable(EnableCap.CullFace);
        GL.Disable(EnableCap.DepthTest);
        GL.Enable(EnableCap.ScissorTest);

        GL.Viewport(0, 0, (int)drawData.DisplaySize.X, (int)drawData.DisplaySize.Y);
        GL.UseProgram(_shaderProgram);
        GL.Uniform1(_attribLocationTex, 0);

        Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, drawData.DisplaySize.X, drawData.DisplaySize.Y, 0, -1.0f, 1.0f);
        GL.UniformMatrix4(_attribLocationProjMtx, false, ref projection);

        GL.BindVertexArray(_vertexArray);
        for (int i = 0; i < drawData.CmdListsCount; i++)
        {
            var cmdList = drawData.CmdLists[i];

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, cmdList.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), cmdList.VtxBuffer.Data, BufferUsageHint.StreamDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, cmdList.IdxBuffer.Size * sizeof(ushort), cmdList.IdxBuffer.Data, BufferUsageHint.StreamDraw);

            int offset = 0;
            for (int cmdi = 0; cmdi < cmdList.CmdBuffer.Size; cmdi++)
            {
                var cmd = cmdList.CmdBuffer[cmdi];
                GL.BindTexture(TextureTarget.Texture2D, (int)cmd.TextureId);
                GL.Scissor(
                    (int)cmd.ClipRect.X,
                    (int)(drawData.DisplaySize.Y - cmd.ClipRect.W),
                    (int)(cmd.ClipRect.Z - cmd.ClipRect.X),
                    (int)(cmd.ClipRect.W - cmd.ClipRect.Y)
                );
                GL.DrawElementsBaseVertex(
                    PrimitiveType.Triangles,
                    (int)cmd.ElemCount,
                    DrawElementsType.UnsignedShort,
                    (IntPtr)(offset * sizeof(ushort)),
                    0
                );
                offset += (int)cmd.ElemCount;
            }
        }

        GL.Disable(EnableCap.ScissorTest);
        GL.BindVertexArray(0);
        GL.UseProgram(0);
    }
}
