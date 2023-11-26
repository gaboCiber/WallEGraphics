using Godot;
using System;
using WallE;
using System.Collections.Generic;

public partial class drawNode : Node2D
{
	List<IFigure> figuresToDraw;
	Vector2 sizeOfThePanel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Configurar el nodo2d
		sizeOfThePanel = ((Window) this.GetParent()).Size;
		this.Position += sizeOfThePanel/2;
        // figuresToDraw = new List<IFigure>
        // {
        //     new Point(0, 0),
		// 	new Line(new Point(0,10), new Point(100,100)),
		// 	new Segment(new Point(0,10), new Point(100,10)),
		// 	new Ray(new Point(10, -10), new Point(50,-10), Ray.Extends.Point1),
		// 	new Point(100,-10),
		// 	new Circle(new Point(50,50), 30),
		// 	new Arc( new Point(-100,-100), 50 , 0, 3*MathF.PI/2)
        // };
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Draw()
    {
        foreach (var item in container.GetFigures())
		{
            if (item is Point point)
            {
                Vector2 vectorIni = ConvertPointToVector(point);
            	Vector2 vectorFin = vectorIni + new Vector2(1f,0f);
            	DrawLine(vectorIni, vectorFin, Colors.Red, 3.0f, true);
            }
			else if(item is Segment segment)
			{
				Vector2 vectorIni = ConvertPointToVector(segment.Point1);
            	Vector2 vectorFin = ConvertPointToVector(segment.Point2);
            	DrawLine(vectorIni, vectorFin, Colors.Yellow, 2.0f, true);
			}
			else if(item is Ray ray)
			{
				Point puntoExtremo1, puntoExtremo2;
				(puntoExtremo1, puntoExtremo2) = EcuationLine(ray);

				Vector2 vectorIni = ConvertPointToVector( (ray.Extension != Ray.Extends.Point1) ? puntoExtremo1 : ray.Point1);
            	Vector2 vectorFin = ConvertPointToVector((ray.Extension != Ray.Extends.Point2) ? puntoExtremo2 : ray.Point2);
            	DrawLine(vectorIni, vectorFin, Colors.DeepPink, 2.0f, true);
			}
			else if(item is Line line)
			{
				Point puntoExtremo1, puntoExtremo2;
				(puntoExtremo1, puntoExtremo2) = EcuationLine(line);

				Vector2 vectorIni = ConvertPointToVector(puntoExtremo1);
            	Vector2 vectorFin = ConvertPointToVector(puntoExtremo2);
            	DrawLine(vectorIni, vectorFin, Colors.DarkViolet, 2.0f, true);			
			}
			else if(item is Circle circle)
			{
				Vector2 center = ConvertPointToVector(circle.Center);
				DrawArc(center, circle.Radio, 0, 2*MathF.PI, 1000, Colors.ForestGreen, 2.0f, true);
			}
			else if(item is Arc arc)
			{
				Vector2 center = ConvertPointToVector(arc.Center);

				DrawArc(center, arc.Radio, -arc.StarAngle, -arc.EndAngle ,1000, Colors.DarkOrange, 2.0f, true);
			}
        }
    }

	private Vector2 ConvertPointToVector(Point figuere) => new Vector2(figuere.X, -figuere.Y);

	private (Point,Point) EcuationLine(Line line)
	{
		float pendiente = (line.Point1.Y - line.Point2.Y)/(line.Point1.X - line.Point2.X);
		float traza = line.Point1.Y - (pendiente * line.Point1.X);
		float lineX = sizeOfThePanel.X/2;

		Point puntoExtremo1 = new Point(lineX, (pendiente*lineX) + traza); 
		Point puntoExtremo2 = new Point(-lineX, -(pendiente*lineX) + traza); 

		return (puntoExtremo1, puntoExtremo2);
	}
}
