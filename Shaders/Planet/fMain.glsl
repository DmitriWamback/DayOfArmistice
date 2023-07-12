#version 410 core

in VERTEX {
    vec3 fragp;
    vec3 normal;
} i;
out vec4 fragc;

void main() {
    fragc = vec4(-i.normal, 1.0);
}