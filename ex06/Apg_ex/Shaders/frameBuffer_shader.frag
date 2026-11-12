#version 330 core
out vec4 FragColor;

in vec2 uv;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform sampler2D texture2;

float shift = 0.001;

float edgeDetection(vec2 uv)
{
    vec4 t0 = texture(texture0, vec2(uv.x - shift, uv.y - shift));
    vec4 t1 = texture(texture0, vec2(uv.x, uv.y - shift));
    vec4 t2 = texture(texture0, vec2(uv.x + shift, uv.y - shift));
    
    vec4 t3 = texture(texture0, vec2(uv.x - shift, uv.y));
    vec4 t4 = texture(texture0, uv);
    vec4 t5 = texture(texture0, vec2(uv.x + shift, uv.y));

    vec4 t6 = texture(texture0, vec2(uv.x - shift, uv.y + shift));
    vec4 t7 = texture(texture0, vec2(uv.x, uv.y + shift));
    vec4 t8 = texture(texture0, vec2(uv.x + shift, uv.y + shift));

    /*
    float l0 = 0.2 * t0.r + 0.7 * t0.g + 0.1 * t0.b;
    float l1 = 0.2 * t1.r + 0.7 * t1.g + 0.1 * t1.b;
    float l2 = 0.2 * t2.r + 0.7 * t2.g + 0.1 * t2.b;

    float l3 = 0.2 * t3.r + 0.7 * t3.g + 0.1 * t3.b;
    float l4 = 0.2 * t4.r + 0.7 * t4.g + 0.1 * t4.b;
    float l5 = 0.2 * t5.r + 0.7 * t5.g + 0.1 * t5.b;

    float l6 = 0.2 * t6.r + 0.7 * t6.g + 0.1 * t6.b;
    float l7 = 0.2 * t7.r + 0.7 * t7.g + 0.1 * t7.b;
    float l8 = 0.2 * t8.r + 0.7 * t8.g + 0.1 * t8.b;
    */

    float l0 = t0.r;
    float l1 = t1.r;
    float l2 = t2.r;

    float l3 = t3.r;
    float l4 = t4.r;
    float l5 = t5.r;

    float l6 = t6.r;
    float l7 = t7.r;
    float l8 = t8.r;

    float laplace = -l0 -l1 -l2
                    -l3 +8*l4 -l5
                    -l6 -l7 -l8;
    laplace = abs(laplace) * 15;
    return laplace;
}

void main()
{
    vec4 tex0 = texture(texture0, uv);
    vec4 tex1 = texture(texture1, uv);
    float l = edgeDetection(uv);

    vec4 outline = mix(tex1, vec4(0.0, 0.0, 0.0, 1.0), l*10);

    vec4 tex2 = texture(texture2, uv);
    float inFront = step(tex0.r, tex2.r);
    vec4 finalColor = mix(outline, tex1, inFront);

    FragColor = finalColor;
}