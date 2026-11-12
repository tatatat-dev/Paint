using Raylib_cs;
using System.Numerics;

Random rng = new Random();

string[] titles =
{
    "Hello world",
    "alividerche",
};
int windowWidth = 800;
int windowHeight = 450;

int circleRadius = 10;

List<(int x ,int y)> Circles = new List<(int, int)> ();
List<(int x, int y, float width, float angle)> ConnectingDots = new List<(int x, int y, float width, float angle)>();

Raylib.InitWindow(windowWidth, windowHeight, titles[rng.Next(0, titles.Length)]);
Raylib.SetTargetFPS(240);

int MouseX;
int MouseY;

int previousDotX = 0;
int previousDotY = 0;
bool isPreviousDot = false;

while (!Raylib.WindowShouldClose())
{
    //1: логика
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        Circles.Clear ();
        ConnectingDots.Clear ();
    }

    MouseX = Raylib.GetMouseX();
    MouseY = Raylib.GetMouseY();

    if (Raylib.IsMouseButtonDown(MouseButton.Left))
    {
        if (!Circles.Contains((MouseX, MouseY)))
        { 
            Circles.Add((MouseX, MouseY));
        }
        if (isPreviousDot)
        {
            ConnectDots(previousDotX, previousDotY, MouseX, MouseY);
        }
        previousDotX = MouseX;
        previousDotY = MouseY;
        isPreviousDot = true;
    }
    if ((Raylib.IsMouseButtonReleased(MouseButton.Left)))
    {
        isPreviousDot = false;
    }



    //2: рисование
    Raylib.BeginDrawing();

    Raylib.ClearBackground(Color.White);

    for (int i = 0; i < Circles.Count; i++)
    {
        Raylib.DrawCircle(Circles[i].x, Circles[i].y, circleRadius, Color.Black);
    }

    for (int i = 0; i < ConnectingDots.Count; i++)
    {
        int cx = ConnectingDots[i].x;
        int cy = ConnectingDots[i].y;
        float length = ConnectingDots[i].width;
        float angle = ConnectingDots[i].angle;
        Raylib.DrawRectanglePro
        (
            new Rectangle(cx, cy, length, circleRadius*2),
            new Vector2(length / 2, circleRadius*2 / 2),
            angle,
            Color.Black
        );
    }



    Raylib.DrawCircle(MouseX, MouseY, circleRadius, Color.Gray);

    Raylib.DrawText($"x: {MouseX} y: {MouseY}", 10, windowHeight - 40, 30, Color.Black);


    Raylib.EndDrawing();
}

Raylib.CloseWindow();


void ConnectDots(int px, int py, int mx, int my)
{
    int dx = mx - px;
    int dy = my - py;
    float length = MathF.Sqrt(dx * dx + dy * dy);
    ConnectingDots.Add(((px + mx) / 2, (py + my) / 2, length, MathF.Atan2(dy, dx) * 180 / MathF.PI));
    

}