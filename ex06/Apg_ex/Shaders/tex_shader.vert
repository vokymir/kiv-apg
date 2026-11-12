#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 8) in vec2 aUv;

out vec2 uv;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    uv = aUv;
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}