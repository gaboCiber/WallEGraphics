using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace WallE
{
    public class CodeProcessor
    {
        string Code;
        List<FigureBase> figures;
        Vector2 size;

        public CodeProcessor(string code, Vector2 sizeOfThePanel)
        {
            Code = code;
            size = sizeOfThePanel;
            Compiling();
        }

        public List<FigureBase> GetFigures() => figures;

        private void Compiling()
        {
       
            figures = new List<FigureBase>
            {
                CreateRamdomPoint(),
                new WallE.Figure.Point(0, 0) { Color = Graphics.GraphicColors.Blue, Tag = "Point"},
                new WallE.Figure.Line(CreateRamdomPoint(), CreateRamdomPoint()) {Tag = "Line"},
                new WallE.Figure.Segment(CreateRamdomPoint(),CreateRamdomPoint()) {Color = Graphics.GraphicColors.Red, Tag = "Segment"},
                new WallE.Figure.Ray( CreateRamdomPoint(), CreateRamdomPoint()) {Color = Graphics.GraphicColors.Red, Tag = "Ray"},
                new WallE.Figure.Circle(CreateRamdomPoint(), CreateRamdomRadius()) {Color = Graphics.GraphicColors.Magenta, Tag = "Circle"},
                new WallE.Figure.Arc(CreateRamdomPoint(), CreateRamdomRadius() , CreateRamdomAngle(), CreateRamdomAngle()) {Color = Graphics.GraphicColors.White, Tag = "arc"},
                //new WallE.Figure.Arc( new Figure.Point(105,-1), 20, MathF.PI/2, 2*MathF.PI + MathF.PI/4) {Color = Graphics.GraphicColors.Yellow, Tag = "The other line"},
                new WallE.Figure.Arc( CreateRamdomPoint(), CreateRamdomPoint(), CreateRamdomPoint(), CreateRamdomRadius()) {Color = Graphics.GraphicColors.Green}
            };
        }

        private WallE.Figure.Point CreateRamdomPoint()
        {
            Random r = new Random();
            int top = (int) Math.Min(size.X, size.Y) / 2;
            return new WallE.Figure.Point(r.Next(-top, top),r.Next(-top, top));
        }

        private float CreateRamdomAngle() => new Random().Next(0, 2 * (int) Math.PI);

        private float CreateRamdomRadius() => new Random().Next(0, (int) Math.Min(size.X, size.Y));

    }
}