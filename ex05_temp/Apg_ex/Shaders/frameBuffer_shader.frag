#version 330 core
out vec4 FragColor;

in vec2 uv;

uniform sampler2D texture0;
uniform sampler2D texture1;

float shift = 0.001;

float edgeDetection(vec2 uv) {
  float res = 0;

  for (int x = -1; x <= 1; ++x) {
    for (int y = -1; y <= 1; ++y) {
      vec4 col = texture(texture0, vec2(uv.x + x * shift, uv.y + y * shift));
      float lightness = (col.r + col.g + col.b) / 3.0;

      if (x == 0 && y == 0) {
        res += 8 * lightness;
      } else {
        res += -1 * lightness;
      }
    }
  }

  return res;
}

void main() {
  vec4 tex0 = texture(texture0, uv); // depth
  vec4 tex1 = texture(texture1, uv); // color
  float l = edgeDetection(uv);

  // TODO outline
  // - based on value of l set to texture color or to edge color
  float isOutline = step(l, 0.0001);

  vec4 outlineCol = vec4(0.0, 1.0, 0.0, 1.0);

  vec4 resCol = mix(outlineCol, tex1, isOutline);

  // resCol = mix(vec4(0), vec4(1), l);

  if (tex0.r < tex1.r) {
    resCol = tex1;
  }

  FragColor = resCol;
}