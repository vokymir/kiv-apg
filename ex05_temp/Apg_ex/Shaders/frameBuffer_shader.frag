#version 330 core
out vec4 FragColor;

in vec2 uv;

uniform sampler2D texture0;
uniform sampler2D texture1;

float shift = 0.001;

float edgeDetection(vec2 uv)
{
    return 0;
}

void main()
{
    vec4 tex0 = texture(texture0, uv);
    vec4 tex1 = texture(texture1, uv);
    float l = edgeDetection(uv);

    // TODO outline
    // - based on value of l set to texture color or to edge color
    vec4 outline = vec4(0);

    FragColor = outline;
}