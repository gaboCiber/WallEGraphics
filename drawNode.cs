using Godot;
using System;
using WallE;
using System.Collections.Generic;

public partial class drawNode : Node2D
{
	static List<IFigure> FiguresToDraw;
	Vector2 sizeOfThePanel;
	float factor = 1;
	Vector2 direction = new Vector2(0,0);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Configurar el nodo2d
		sizeOfThePanel = ((Window) this.GetParent()).Size;

		this.Position += sizeOfThePanel/2;

		GetNode<Button>("mas").Position = 0.8f * sizeOfThePanel / 2;
		GetNode<Button>("menos").Position = 0.8f * sizeOfThePanel / 2 + new Vector2(30,0);
		GetNode<Button>("right").Position = new Vector2( -sizeOfThePanel.X / 2 * 0.8f, sizeOfThePanel.Y/2 * 0.75f ) ;
		GetNode<Button>("left").Position = new Vector2( -sizeOfThePanel.X / 2 * 0.9f, sizeOfThePanel.Y/2 * 0.75f );
		GetNode<Button>("up").Position = new Vector2( -sizeOfThePanel.X / 2 * 0.85f, sizeOfThePanel.Y/2 * 0.65f );
		GetNode<Button>("down").Position = new Vector2( -sizeOfThePanel.X / 2 * 0.85f, sizeOfThePanel.Y/2 * 0.85f );
		GetNode<Button>("center").Position = new Vector2( -sizeOfThePanel.X / 2 * 0.85f, sizeOfThePanel.Y/2 * 0.75f );


		GetNode<Button>("mas").Pressed += MasPressed;
		GetNode<Button>("menos").Pressed += MenosPressed;
		GetNode<Button>("right").Pressed += RightPressed;
		GetNode<Button>("left").Pressed += LeftPressed;
		GetNode<Button>("up").Pressed += UpPressed;
		GetNode<Button>("down").Pressed += DownPressed;
		GetNode<Button>("center").Pressed += CenterPressed;
    }

	public static void AddFigures(List<IFigure> figuresToDraw)
	{
		FiguresToDraw = figuresToDraw;
	}

    public override void _Draw()
    {
        foreach (var item in FiguresToDraw)
		{
            if (item is Point point)
            {
                Vector2 vectorIni = ConvertPointToVector(point);
            	Vector2 vectorFin = vectorIni + new Vector2(1f,0f);
            	DrawLine(vectorIni * factor + direction, vectorFin * factor + direction, Colors.Red, 3.0f, true);
            }
			else if(item is Segment segment)
			{
				Vector2 vectorIni = ConvertPointToVector(segment.Point1);
            	Vector2 vectorFin = ConvertPointToVector(segment.Point2);
            	DrawLine(vectorIni * factor + direction, vectorFin * factor + direction, Colors.Yellow, 2.0f, true);
			}
			else if(item is Ray ray)
			{
				Point puntoExtremo1, puntoExtremo2;
				(puntoExtremo1, puntoExtremo2) = EcuationLine(ray);

				Vector2 vectorIni = ConvertPointToVector( (ray.Extension != Ray.Extends.Point1) ? puntoExtremo1 : ray.Point1);
            	Vector2 vectorFin = ConvertPointToVector((ray.Extension != Ray.Extends.Point2) ? puntoExtremo2 : ray.Point2);
            	DrawLine(vectorIni * factor + direction, vectorFin * factor + direction, Colors.DeepPink, 2.0f, true);
			}
			else if(item is Line line)
			{
				Point puntoExtremo1, puntoExtremo2;
				(puntoExtremo1, puntoExtremo2) = EcuationLine(line);

				Vector2 vectorIni = ConvertPointToVector(puntoExtremo1);
            	Vector2 vectorFin = ConvertPointToVector(puntoExtremo2);
            	DrawLine(vectorIni * factor + direction, vectorFin * factor + direction, Colors.DarkViolet, 2.0f, true);			
			}
			else if(item is Circle circle)
			{
				Vector2 center = ConvertPointToVector(circle.Center);
				DrawArc(center * factor + direction, circle.Radio * factor, 0, 2*MathF.PI, 1000, Colors.ForestGreen, 2.0f, true);
			}
			else if(item is Arc arc)
			{
				Vector2 center = ConvertPointToVector(arc.Center);

				DrawArc(center * factor + direction, arc.Radio * factor, -arc.StarAngle, -arc.EndAngle ,1000, Colors.DarkOrange, 2.0f, true);
			}
        }
    }

	private Vector2 ConvertPointToVector(Point figuere) => new Vector2(figuere.X, -figuere.Y);

	private (Point,Point) EcuationLine(Line line)
	{
		float pendiente = (line.Point1.Y - line.Point2.Y)/(line.Point1.X - line.Point2.X);
		float traza = line.Point1.Y - (pendiente * line.Point1.X);
		float lineX = sizeOfThePanel.X/2 / factor - direction.X;

		Point puntoExtremo1 = new Point(lineX, (pendiente*lineX) + traza); 
		Point puntoExtremo2 = new Point(-lineX, -(pendiente*lineX) + traza); 

		return (puntoExtremo1, puntoExtremo2);
	}

	public void MasPressed()
	{
		factor += 0.1f;
		GetNode<Button>("menos").Disabled = false;
		QueueRedraw();
	}

	public void MenosPressed()
	{
		if( factor <= 0.1)
			GetNode<Button>("menos").Disabled = true;
		else
		{
			factor -= 0.1f;
			QueueRedraw();
		}
	}

	public void RightPressed()
	{
		direction += new Vector2(-1,0);
		QueueRedraw();
	}

	public void LeftPressed()
	{
		direction += new Vector2(1,0);
		QueueRedraw();
	}

	public void UpPressed()
	{
		direction += new Vector2(0,1);
		QueueRedraw();
	}

	public void DownPressed()
	{
		direction += new Vector2(0,-1);
		QueueRedraw();
	}

	public void CenterPressed()
	{
		direction = new Vector2(0,0);
		QueueRedraw();
	}
}
