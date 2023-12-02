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
                new Point(0, 0),
                new Line(CreateRamdomPoint(), CreateRamdomPoint()),
                new Segment(CreateRamdomPoint(),CreateRamdomPoint()),
                new Ray(CreateRamdomPoint(), CreateRamdomPoint(), Ray.Extends.Point2),
                new Circle(CreateRamdomPoint(), CreateRamdomRadius()),
                new Arc(CreateRamdomPoint(), CreateRamdomRadius() , CreateRamdomAngle(), CreateRamdomAngle())
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