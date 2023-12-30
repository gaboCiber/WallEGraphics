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
        List<string> ErrorList, OutputList;
        Vector2 size;
        public bool IsThereAnyErrors { get => ErrorList.Count != 0;  }

        public CodeProcessor(string code, Vector2 sizeOfThePanel)
        {
            Code = code;
            size = sizeOfThePanel;
            FiguresList = new List<FigureBase>();
            ErrorList = new List<string>();
            OutputList = new List<string>();
            
            CompilingAndErrors();
        }

        public List<FigureBase> GetFigures() => FiguresList;

        public List<string> GetErrors() => ErrorList;

        public List<string> GetOutput() => OutputList;

        
        private void CompilingAndErrors()
        {
            (List<Figure> figures , List<string> errors ) comp = ( new List<Figure>(), new List<string>());
            comp = Operation_System.Validate_Program(Code);

            if(comp.errors is not null)
            {
                ErrorList = comp.errors;
                return;
            }

            foreach (var figure in comp.figures)
            {
                if(IsThereAnyErrors)
                    break;

                if(figure is Point point)
                    FiguresList.Add(PointGConversion(point));
                else if(figure is Ray ray)
                    FiguresList.Add(RayGConversion(ray));
                else if(figure is Segment segment)
                    FiguresList.Add(SegmentGConversion(segment));
                else if(figure is Line line)
                    FiguresList.Add(LineGConversion(line));
                else if(figure is Arc arc)
                   FiguresList.Add(ArcGConversion(arc));
                else if(figure is Circle circle)
                    FiguresList.Add(CircleGConversion(circle));
                else if(figure is Printer print)
                    OutputList.Add(print.Value);
            } 


        }
        private WallE.Graphics.GraphicColors ColorsGConvertion(string color)
        {
            if(Enum.TryParse<GraphicColors>( color, true, out var colorG))
                return colorG;
            else if( color == string.Empty || color is null)
                return GraphicColors.black;
            else if(color == "magent")
                return GraphicColors.magenta;
            else
            {
                ErrorList.Add($"Sematic Error!!: '{color}' no es un color valido");
                return GraphicColors.black;;
            }
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