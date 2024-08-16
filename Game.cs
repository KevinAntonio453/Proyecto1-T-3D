using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
//using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Diagnostics;
using LearnOpenTK.Common;


namespace PGrafica1
{
    public class Game : GameWindow // extender la class Gamewindows
    {
        // matriz de vertices para cada una de las caras que componen la letra T(3D)
        // cada cara se compone de dos triangulos
        //por lo que cada cara tendra 6 vertices
        private readonly float[] vertices =
       {
           // Base
            -0.1f,  0.0f, -0.1f, // Cara frontal
             0.1f,  0.0f, -0.1f,
             0.1f, -0.5f, -0.1f,
            -0.1f,  0.0f, -0.1f,
            -0.1f, -0.5f, -0.1f,
             0.1f, -0.5f, -0.1f,

             -0.1f,  0.0f, 0.1f, // Cara posterior
              0.1f,  0.0f, 0.1f,
              0.1f, -0.5f, 0.1f,
             -0.1f,  0.0f, 0.1f,
             -0.1f, -0.5f, 0.1f,
              0.1f, -0.5f, 0.1f,
              //       y     x
             -0.1f,  0.0f,  0.1f, /// Cara izquierda
             -0.1f,  0.0f, -0.1f,
             -0.1f, -0.5f, -0.1f,
             -0.1f, -0.5f, -0.1f,
             -0.1f, -0.5f,  0.1f,
             -0.1f,  0.0f,  0.1f,

              0.1f,  0.0f,  0.1f, /// derecha
              0.1f,  0.0f, -0.1f,
              0.1f, -0.5f, -0.1f,
              0.1f, -0.5f, -0.1f,
              0.1f, -0.5f,  0.1f,
              0.1f,  0.0f,  0.1f,

             -0.1f,  0f, -0.1f, /// superior
              0.1f,  0f, -0.1f,
              0.1f,  0f,  0.1f,
              0.1f,  0f,  0.1f,
             -0.1f,  0f,  0.1f,
             -0.1f,  0f, -0.1f,

            -0.1f, -0.5f, -0.1f, // Cara inferior
             0.1f, -0.5f, -0.1f,
             0.1f, -0.5f,  0.1f,
             0.1f, -0.5f,  0.1f,
            -0.1f, -0.5f,  0.1f,
            -0.1f, -0.5f, -0.1f,

            //Parte Superior
            -0.3f,  0.0f,  0.1f, // Cara frontal
            -0.3f,  0.15f, 0.1f,
             0.3f,  0.15f, 0.1f,
            -0.3f,  0.0f,  0.1f,
             0.3f,  0.0f,  0.1f,
             0.3f,  0.15f, 0.1f,

            -0.3f,  0.0f,  -0.1f, // Cara posterior
            -0.3f,  0.15f, -0.1f,
             0.3f,  0.15f, -0.1f,
            -0.3f,  0.0f,  -0.1f,
             0.3f,  0.0f,  -0.1f,
             0.3f,  0.15f, -0.1f,

             -0.3f,  0.15f,  0.1f, /// Cara izquierda
             -0.3f,  0.15f, -0.1f,
             -0.3f,  0.0f,  -0.1f,
             -0.3f,  0.0f,  -0.1f,
             -0.3f,  0.0f,   0.1f,
             -0.3f,  0.15f,  0.1f,

              0.3f,  0.15f,  0.1f, /// derecha
              0.3f,  0.15f, -0.1f,
              0.3f, -0.0f, -0.1f,
              0.3f, -0.0f, -0.1f,
              0.3f, -0.0f,  0.1f,
              0.3f,  0.15f,  0.1f,

             -0.3f,  -0.0f, -0.1f, /// superior
              0.3f,  -0.0f, -0.1f,
              0.3f,  -0.0f,  0.1f,
              0.3f,  -0.0f,  0.1f,
             -0.3f,  -0.0f,  0.1f,
             -0.3f,  -0.0f, -0.1f,

            -0.3f, 0.15f, -0.1f, // Cara inferior
             0.3f, 0.15f, -0.1f,
             0.3f, 0.15f,  0.1f,
             0.3f, 0.15f,  0.1f,
            -0.3f, 0.15f,  0.1f,
            -0.3f, 0.15f, -0.1f,
        };


        int identificador_de_bufer_de_vertices;// Objeto búfer de vértice_vertexBufferObject;
        private int elementBufferObject;
        private Camera Camara;
        private bool _firstMove = true;
        private int _vaoModel;// modelo de el objeto Matriz de Vertices
        private Shader shader;
        private Vector2 _lastPos;
         // Luego, creamos dos matrices para mantener nuestra vista y proyección. Se inicializan al final de OnLoad.
        // La matriz de vista es lo que podrías considerar la «cámara». Representa la vista actual en la ventana.
        private Matrix4 vista;
        // Esto representa como serán proyectados los vértices. 
        private Matrix4 Proyeccion;
        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }
        // Ahora, empezamos a inicializar OpenGL.
        protected override void OnLoad()// este metodo carga el formulario
        {
            base.OnLoad();
            GL.ClearColor(new Color4(0.2f,0.3f,0.3f,1.0f));
            GL.Enable(EnableCap.DepthTest);

            this.identificador_de_bufer_de_vertices = GL.GenBuffer();//crea el bufer vacio
            // vinculamos el buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.identificador_de_bufer_de_vertices);   
            // cargamos los vertices al buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),vertices, BufferUsageHint.StaticDraw);

            shader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            
                // Inicializamos el objeto matriz de vertices
                _vaoModel = GL.GenVertexArray();
                GL.BindVertexArray(_vaoModel);
                var vertexLocation = shader.GetAttribLocation("aPos");
                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);


            Camara = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
            CursorState = CursorState.Grabbed;


        }
        protected override void OnRenderFrame(FrameEventArgs args)//marco de renderizado
        {  
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindVertexArray(_vaoModel);
            shader.Use();

            // Matrix4.Identity se utiliza como la matriz, ya que sólo queremos dibujar en 0, 0, 0
            shader.SetMatrix4("model", Matrix4.Identity);
            shader.SetMatrix4("view", Camara.GetViewMatrix());
            shader.SetMatrix4("projection", Camara.GetProjectionMatrix());

            shader.SetVector3("objectColor", new Vector3(0.26f, 0.85f, 0.15f));
            shader.SetVector3("lightColor", new Vector3(0.0f, 1.0f, 1.0f));
            // dibujar la matriz con un total de 72 verices
            GL.DrawArrays(PrimitiveType.Triangles, 0, 72);
            SwapBuffers();

        }
        //Un framebuffer es una porción de memoria que se utiliza para almacenar una imagen
        //antes de que se muestre en la pantalla1. Cada píxel de la pantalla se representa como una
        //ubicación en esta memoria de acceso aleatorio.
        //En el contexto de OpenGL, un framebuffer es un objeto que facilita la renderización de gráficos,
        protected override void OnUpdateFrame(FrameEventArgs e)//al actualizar el marco
        {
            base.OnUpdateFrame(e);
            if (!IsFocused) //no esta enfocado?
            {
                return;
            }

            var input = KeyboardState;
            if (KeyboardState.IsKeyDown(Keys.Escape))//si presiona la tecla escape
            {
                Close();// la ventana se cierra
            }
            const float velocidadCamara = 1.5f;
            const float sensibilidad = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                Camara.Position += Camara.Front * velocidadCamara * (float)e.Time; // Adelante
            }

            if (input.IsKeyDown(Keys.S))
            {
                Camara.Position -= Camara.Front * velocidadCamara * (float)e.Time; // Hacia atrás
            }
            if (input.IsKeyDown(Keys.A))
            {
                Camara.Position -= Camara.Right * velocidadCamara * (float)e.Time; // Izquierda
            }
            if (input.IsKeyDown(Keys.D))
            {
                Camara.Position += Camara.Right * velocidadCamara * (float)e.Time; // derecha
            }
            if (input.IsKeyDown(Keys.Space))
            {
                Camara.Position += Camara.Up * velocidadCamara * (float)e.Time; // arriba
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                Camara.Position -= Camara.Up * velocidadCamara * (float)e.Time; // abajo
            }

            // Obtener el estado del ratón
            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calcula el desplazamiento de la posición del ratón
                var X = mouse.X - _lastPos.X;
                var Y = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                Camara.Yaw += X * sensibilidad;
                Camara.Pitch -= Y * sensibilidad; 
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            Camara.Fov -= e.OffsetY;
        }
        protected override void OnResize(ResizeEventArgs e)//al cambiar el tamano
        {
            // Cuando la ventana se redimensiona, tenemos que llamar a GL.Viewport para
            // redimensionar la ventana de OpenGL para que coincida con el nuevo tamaño.
            // Si no lo hacemos, el NDC dejará de ser correcto.

            base.OnResize(e);
            GL.Viewport(0,0,e.Width,e.Height);
            Camara.AspectRatio = Size.X / (float)Size.Y;
        }
        protected override void OnUnload()
        {
       
            base.OnUnload();
        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            //Con GL.Viewportpodemos actualizar esta transformación para que la renderización
            //se realice correctamente en todo el framebuffer.
        }
    }
}
