using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib; //хз почему статик


int width = 800;
int height = 450;
InitWindow(width, height, "Paint");

SetTargetFPS(60);

int xolstY = 100;
int xolstheight = 300;
int xolstBottom = xolstY + xolstheight;

RenderTexture2D xolst = LoadRenderTexture(width, xolstheight);

BeginTextureMode(xolst);
ClearBackground(Color.White);
EndTextureMode();

int brushRadius = 15;
Color currentColor = Color.Gray;

bool isPastCircle = false;
int pastMouseX = 0;
int pastMouseY = 0;

List<ColorButton> ColorButtons = new List<ColorButton>()
{
    new ColorButton {bounds = new Rectangle(100,30,30,30), color = Color.Red},
    new ColorButton {bounds = new Rectangle(140,30,30,30), color = Color.Blue},
    new ColorButton {bounds = new Rectangle(180,30,30,30), color = Color.Green},
    new ColorButton {bounds = new Rectangle(220,30,30,30), color = Color.Yellow},
    new ColorButton {bounds = new Rectangle(260,30,30,30), color = Color.Black},
    new ColorButton {bounds = new Rectangle(300,30,30,30), color = Color.White}
};

Slider brushSize = new Slider { start = 500, end = 700, currentPos = (700+500)/2, posY = 45, sliderRadius = brushRadius, color = Color.Blue};

while (!WindowShouldClose())
{

    if (IsMouseButtonReleased(MouseButton.Left)) isPastCircle = false;

    if (IsMouseButtonPressed(MouseButton.Left))
    {
        foreach (ColorButton btn in ColorButtons)
        {
            if (btn.OnClick() != null)
            {
                currentColor = btn.color;
            }
        }
        brushSize.IsDragging();
    }
    if (IsMouseButtonDown(MouseButton.Left))
    {

        if (brushSize.isDragging) brushRadius = brushSize.Dragging();
        else
        {
            Draw();
            countPastThings();
        }
    }


    BeginDrawing();

    ClearBackground(Color.Black);

    DrawXolst();

    DrawBrushPreview();

    DrawBounds();

    DrawColorButtons();

    DrawCursorCoordinates();

    brushSize.DrawSlider();

    EndDrawing();
}

UnloadRenderTexture(xolst);
CloseWindow();

void Draw()
{

        BeginTextureMode(xolst);
        DrawCircle(GetMouseX(), GetMouseY() - xolstY, brushRadius, currentColor);
        if (isPastCircle)
        {
            ConnectCircles();
        }
        EndTextureMode();

}
void DrawColorButtons()
{
    foreach(ColorButton btn in ColorButtons)
    {
        btn.Draw();
    }
}

void DrawCursorCoordinates()
{
    if (GetMouseY() < xolstY || GetMouseY() > xolstBottom) return;
    DrawText($"x: {GetMouseX()} y: {GetMouseY() - xolstY}", 20, 405,40,Color.Black);
}
void ConnectCircles()
{
    int dx = GetMouseX() - pastMouseX;
    int dy = (GetMouseY() - xolstY) - pastMouseY;
    float recWidth = MathF.Sqrt(dx * dx + dy * dy);
    Rectangle rec = new Rectangle((pastMouseX + GetMouseX()) / 2, ((pastMouseY) + (GetMouseY()-xolstY)) / 2, recWidth, brushRadius * 2);
    Vector2 origin = new Vector2(recWidth / 2, brushRadius);
    float rotation = MathF.Atan2(dy, dx)*180/MathF.PI;
    DrawRectanglePro(rec, origin, rotation, currentColor);
        
}

void countPastThings()
{
    isPastCircle = true;
    pastMouseX = GetMouseX();
    pastMouseY = GetMouseY() - xolstY;
}
void DrawXolst()
{
    /* непонятная хрень,почему тут new, после классов вернуться и посмотреть*/
    Rectangle source = new Rectangle(0, 0, width, -xolstheight); //что я беру с холста, то что буду вырисовывать
    Rectangle dest = new Rectangle(0, xolstY, width, xolstheight); //куда холст всунуть, и масштабирование
    Vector2 origin = new Vector2(0, 0); //чтобы место вращения поставить
    DrawTexturePro(xolst.Texture, source, dest, origin, 0f, Color.White);
}

void DrawBounds()
{

    DrawRectangle(0, 0, width, xolstY, new Color(230, 242, 255, 255));
    DrawRectangle(0, xolstBottom, width, height - xolstBottom, new Color(230, 242, 255, 255));

}

void DrawBrushPreview()
{
    DrawCircle(GetMouseX(), GetMouseY(), brushRadius, currentColor);
}

struct ColorButton() //пишу в первый раз, изучить детальнее
{
    public Rectangle bounds;
    public Color color;

    public void Draw()
    {
        DrawRectangleRec(bounds, color);
    }
    public Color? OnClick()
    {
        int mouseX = GetMouseX();
        int mouseY = GetMouseY();
        if (mouseX >= bounds.X && mouseY >= bounds.Y && mouseX <= bounds.X + bounds.Width && mouseY <= bounds.Y + bounds.Height)
        {
            return color;
        }
        return null;
    }
}

struct Slider()
{
    public int start;
    public int end;
    public int currentPos;
    public int posY;
    public int sliderRadius;
    public Color color;
    public bool isDragging = false;

    public void IsDragging()
    {
        int mouseX = GetMouseX();
        int mouseY = GetMouseY();
        if (mouseY >= posY - sliderRadius && mouseY <= posY + sliderRadius && mouseX <= currentPos+sliderRadius && mouseX >= currentPos-sliderRadius )
        {
            isDragging = true;
        }
        else isDragging = false;
    }

    public int Dragging()
    {
        int mouseX = GetMouseX();
        currentPos = mouseX;
        return ((currentPos - start) / 2 + 1);
    }

    public void DrawSlider()
    {
        DrawRectangle(start, posY-posY/4, end - start, posY/2, Color.DarkBlue);
        if (currentPos < start) currentPos = start;
        if (currentPos > end) currentPos = end;
        DrawCircle(currentPos, posY, sliderRadius, color);
    }
}