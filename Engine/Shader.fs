
  
#version 330 core
out vec4 FragColor;

in vec3 ourColor;
in vec2 TexCoord;

// texture sampler
uniform sampler2D texture1;

void main()
{
	FragColor = texture(texture1, TexCoord);
    //FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
    //FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
    //FragColor = vec4(ourColor, 1.0);
}