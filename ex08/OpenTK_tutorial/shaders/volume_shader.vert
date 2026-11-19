#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 2) in vec3 aNormal;
layout (location = 8) in vec2 aUv;

out vec2 uv;

void main()
{
    uv = aUv;
    gl_Position = vec4(aPosition, 1.0);
}