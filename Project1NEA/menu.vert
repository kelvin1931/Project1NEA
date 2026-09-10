#version 330 core

layout(location = 0) in vec2 aPosition;

uniform vec2 offset;
uniform vec2 scale;

void main()
{
    gl_Position = vec4(aPosition * scale + offset, 0.0, 1.0);
}
