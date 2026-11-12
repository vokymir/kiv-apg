#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 2) in vec3 aNormal;
layout (location = 8) in vec2 aUv;

out vec2 uv;
uniform mat4 model;

void main()
{
    uv = vec2(aUv.x, 1.0 - aUv.y);
    gl_Position = vec4(aPosition, 1.0) * model;
}