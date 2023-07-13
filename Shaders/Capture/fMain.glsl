#version 410 core

uniform sampler2D tex;

in VERTEX {
    vec3 fragp;
    vec2 uv;
} i;
out vec4 fragc;

void main() {
    fragc = texture(tex, i.uv);
}