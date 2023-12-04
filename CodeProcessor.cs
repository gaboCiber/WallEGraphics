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
        List<IFigure> figures;
        Vector2 size;

        public CodeProcessor(string code, Vector2 sizeOfThePanel)
        {
            Code = code;
            size = sizeOfThePanel;
            Compiling();
        }

        public List<IFigure> GetFigures() => figures;

        private void Compiling()
        {
       
            figures = new List<IFigure>
            {
                CreateRamdomPoint(),
                new Point(0, 0) { Color = Colors.Beige, Tag = "Point"},
                new Line(CreateRamdomPoint(), CreateRamdomPoint()) {Color = Colors.MediumVioletRed, Tag = "Line"},
                new Segment(CreateRamdomPoint(),CreateRamdomPoint()) {Color = Colors.DarkGreen, Tag = "Segment"},
                new Ray( CreateRamdomPoint(), CreateRamdomPoint()) {Color = Colors.Yellow, Tag = "Ray"},
                new Circle(CreateRamdomPoint(), CreateRamdomRadius()) {Color = Colors.Crimson, Tag = "Circle"},
                new Arc(CreateRamdomPoint(), CreateRamdomRadius() , 0, MathF.PI) {Color = Colors.Olive, Tag = "arc"},
            };
        }

        private Point CreateRamdomPoint()
        {
            Random r = new Random();
            int top = (int) Math.Min(size.X, size.Y) / 2;
            return new Point(r.Next(0, top),r.Next(0, top));
        }

        private float CreateRamdomAngle() => new Random().Next(0, (int) Math.PI);

        private float CreateRamdomRadius() => new Random().Next(0, (int) Math.Min(size.X, size.Y));

    }
}