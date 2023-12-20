using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using WallE.Graphics;

namespace WallE
{
    public class CodeProcessor
    {
        string Code;
        List<FigureBase> FiguresList;
         List<string> ErrorList;
        Vector2 size;
        public bool IsThereAnyErrors { get => ErrorList.Count != 0;  }

        public CodeProcessor(string code, Vector2 sizeOfThePanel)
        {
            Code = code;
            size = sizeOfThePanel;
            FiguresList = new List<FigureBase>();
            ErrorList = new List<string>();
            
            Compiling();

            // ErrorList.AddRange( new string[]{
            //     "dasas as dsaas",
            //     "ad asd asda asd", 
            //     "adsasdasd asd asd asdas",
            //     "asin dlas dpasdmals das dnasli dasldasldasl daosdlnsla asldasldasldnasldnasldasldasdlnals dnalsd nas ldl  askdn",
            //     "na sdolnasdlnaspdasdlasdp apsnas ans d nasndoayf biergnroe gnorg  onfo welfnaldn LNQNGLD" 
            // } );
        }

        public List<FigureBase> GetFigures() => FiguresList;

        public List<string> GetErrors() => ErrorList;

        private void Compiling()
        {
            foreach (var figure in Operation_System.Validate_Program(Code))
            {
                if(figure is Point point)
                    FiguresList.Add(PointGConversion(point));
                else if(figure is Ray ray)
                    FiguresList.Add(RayGConversion(ray));
                else if(figure is Segment segment)
                    FiguresList.Add(SegmentGConversion(segment));
                else if(figure is Line line)
                    FiguresList.Add(LineGConversion(line));
                // else if(figure is Arc arc)
                //    FiguresList.Add(ArcGConversion(arc));
                else if(figure is Circle circle)
                    FiguresList.Add(CircleGConversion(circle));
            }
          
            // FiguresList = new List<FigureBase>
            // {
            //     CreateRamdomPoint(),
            //     new WallE.FigureGraphics.Point(0, 0) { Color = Graphics.GraphicColors.Blue, Tag = "Point"},
            //     new WallE.FigureGraphics.Line(CreateRamdomPoint(), CreateRamdomPoint()) {Tag = "Line"},
            //     new WallE.FigureGraphics.Segment(CreateRamdomPoint(),CreateRamdomPoint()) {Color = Graphics.GraphicColors.Red, Tag = "Segment"},
            //     new WallE.FigureGraphics.Ray( CreateRamdomPoint(), CreateRamdomPoint()) {Color = Graphics.GraphicColors.Red, Tag = "Ray"},
            //     new WallE.FigureGraphics.Circle(CreateRamdomPoint(), CreateRamdomRadius()) {Color = Graphics.GraphicColors.Magenta, Tag = "Circle"},
            //     new WallE.FigureGraphics.Arc(CreateRamdomPoint(), CreateRamdomRadius() , CreateRamdomAngle(), CreateRamdomAngle()) {Color = Graphics.GraphicColors.White, Tag = "arc"},
            //     //new WallE.Figure.Arc( new Figure.Point(105,-1), 20, MathF.PI/2, 2*MathF.PI + MathF.PI/4) {Color = Graphics.GraphicColors.Yellow, Tag = "The other line"},
            //     new WallE.FigureGraphics.Arc( CreateRamdomPoint(), CreateRamdomPoint(), CreateRamdomPoint(), CreateRamdomRadius()) {Color = Graphics.GraphicColors.Green}
            // };

        }

        private WallE.Graphics.GraphicColors ColorsGConvertion(string color)
        {
            if(Enum.TryParse<GraphicColors>( color, true, out var colorG))
                return colorG;
            else
                throw new Exception($"The color {color} is not valid");
        }
        
        private WallE.FigureGraphics.Point PointGConversion(Point point)
        {           

            return new WallE.FigureGraphics.Point( (float) point.X, (float) point.Y)
                { Color = ColorsGConvertion(point.color)};
        }
        
        private WallE.FigureGraphics.Segment SegmentGConversion(Segment segment)
        {
            return new FigureGraphics.Segment( PointGConversion(segment.P1), PointGConversion(segment.P2))
                 { Color = ColorsGConvertion(segment.color)};
            ;
        }
        
        private WallE.FigureGraphics.Ray RayGConversion(Ray ray)
        {
            return new FigureGraphics.Ray( PointGConversion(ray.P1), PointGConversion(ray.P2))
                { Color = ColorsGConvertion(ray.color)};
        }
        
        private WallE.FigureGraphics.Line LineGConversion(Line line)
        {
            return new FigureGraphics.Line( PointGConversion(line.P1), PointGConversion(line.P2))
                { Color = ColorsGConvertion(line.color)};
        }

        private  WallE.FigureGraphics.Circle CircleGConversion(Circle circle)
        {
            return new WallE.FigureGraphics.Circle( PointGConversion(circle.center) , (float) circle.radio.Get_Distance())
                { Color = ColorsGConvertion(circle.color)};
        }

        private  WallE.FigureGraphics.Arc ArcGConversion(Arc arc)
        {
            return new WallE.FigureGraphics.Arc( PointGConversion(arc.center) , PointGConversion(arc.initial) , PointGConversion(arc.final), (float) arc.radio.Get_Distance())
                { Color = ColorsGConvertion(arc.color)};
        }

        //----------------------------------------------------------//
        
        private WallE.FigureGraphics.Point CreateRamdomPoint()
        {
            Random r = new Random();
            int top = (int) Math.Min(size.X, size.Y) / 2;
            return new WallE.FigureGraphics.Point(r.Next(-top, top),r.Next(-top, top));
        }

        private float CreateRamdomAngle() => new Random().Next(0, 2 * (int) Math.PI);

        private float CreateRamdomRadius() => new Random().Next(0, (int) Math.Min(size.X, size.Y));

    }
}