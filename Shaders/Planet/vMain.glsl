#version 410 core

layout(location=0) in vec3 fragp;
layout(location=1) in vec3 normal;

out VERTEX {
    vec3 fragp;
    vec3 normal;
} o;

uniform mat4 projection;
uniform mat4 lookAt;
uniform mat4 model;

void main() {
    o.normal = normal;
    o.fragp = fragp;
    gl_Position = projection * lookAt * vec4(fragp, 1.0);
}