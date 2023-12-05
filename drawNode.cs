using Godot;
using System;
using WallE;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

public partial class drawNode : Node2D
{
	static List<IFigure> FiguresToDraw;
	Vector2 sizeOfThePanel;
	float factor;
	Vector2 direction;
	Font font;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		factor = 1;
		direction = new Vector2(0,0);
		font = new Label().GetThemeFont("Times New Romance.otf");

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
				DrawCircle(vectorIni * factor + direction, 2 * factor,point.Color);
				DrawString(font, new Vector2(5,0) + vectorIni * factor + direction, point.Tag, HorizontalAlignment.Left, -1, (int) (10 * factor) );
			}
			else if(item is Segment segment)
			{
				Vector2 vectorIni = ConvertPointToVector(segment.Point1);
            	Vector2 vectorFin = ConvertPointToVector(segment.Point2);
            	DrawLine(vectorIni * factor + direction, vectorFin * factor + direction, segment.Color, 2.0f, true);
				DrawString(font, new Vector2(5,0) + vectorIni * factor + direction, segment.Tag, HorizontalAlignment.Left, -1, (int) (10 * factor) );
			}
			else if(item is Ray ray)
			{
				(Point puntoExtremo1, Point puntoExtremo2) = IntersectLine(ray);

				Vector2 vectorIni = ConvertPointToVector(ray.Point1);
				Vector2 vectorFin = ConvertPointToVector(IntersectRay(ray));

	        	DrawLine(vectorIni * factor + direction, vectorFin * factor + direction, ray.Color, 2.0f, true);
				DrawString(font, new Vector2(5,0) + vectorIni * factor + direction, ray.Tag, HorizontalAlignment.Left, -1, (int) (10 * factor) );
			}
			else if(item is Line line)
			{
				(Point puntoExtremo1, Point puntoExtremo2) = IntersectLine(line);

				Vector2 vectorIni = ConvertPointToVector(puntoExtremo1);
            	Vector2 vectorFin = ConvertPointToVector(puntoExtremo2);
            	
				DrawLine(vectorIni * factor + direction, vectorFin * factor + direction, line.Color, 2.0f, true);			
				DrawString(font, new Vector2(5,0) + ConvertPointToVector(line.Point1) * factor + direction, line.Tag, HorizontalAlignment.Left, -1, (int) (10 * factor) );
			}
			else if(item is Circle circle)
			{
				Vector2 center = ConvertPointToVector(circle.Center);
				DrawArc(center * factor + direction, circle.Radio * factor, 0, 2*MathF.PI, 1000, circle.Color, 2.0f, true);
				DrawString(font, (center + new Vector2(circle.Radio + 5,0)) * factor + direction, circle.Tag, HorizontalAlignment.Left, -1, (int) (10 * factor) );
			}
			else if(item is Arc arc)
			{
				Vector2 center = ConvertPointToVector(arc.Center);
				Vector2 middlePoint = ConvertPointToVector(ArcMiddlePoint(arc)) + new Vector2(10,10);
				DrawArc(center * factor + direction, arc.Radio * factor, -arc.StarAngle, -arc.EndAngle ,1000, arc.Color, 2.0f, true);
				DrawString(font, (center + middlePoint) * factor + direction, arc.Tag, HorizontalAlignment.Left, -1, (int) (10 * factor) );
				
			}
        }
    }

	private Vector2 ConvertPointToVector(Point point) => new Vector2(point.X, -point.Y);

	private (Point,Point) IntersectLine(Line line)
	{
		Point puntoExtremo1, puntoExtremo2;

		if(line.Point1.X == line.Point2.X)
		{
			if(line.Point1.Y == line.Point2.Y)
				throw new Exception("A line is given by two points");
		

			float lineParalleToX = sizeOfThePanel.Y/2 / factor - direction.X;
			puntoExtremo1 = new Point(line.Point1.X, lineParalleToX); 
			puntoExtremo2 = new Point(line.Point1.X, -lineParalleToX); 
		}
		else
		{
			float lineParalleToY = sizeOfThePanel.X/2 / factor - direction.X;
			float pendiente = (line.Point1.Y - line.Point2.Y)/(line.Point1.X - line.Point2.X);
			float traza = line.Point1.Y - (pendiente * line.Point1.X);
			
			puntoExtremo1 = new Point( lineParalleToY,(pendiente*lineParalleToY) + traza); 
			puntoExtremo2 = new Point(-lineParalleToY, -(pendiente*lineParalleToY) + traza); 
		}

		return (puntoExtremo1, puntoExtremo2);
	}

	private Point IntersectRay(Ray ray)
	{	
		Vector2 vector = new Vector2( ray.Point2.X - ray.Point1.X ,ray.Point2.Y - ray.Point1.Y);

		if(vector.X == 0 && vector.Y == 0)
				throw new Exception("In a ray, the passing through point must be diferent that the starting point");
			

		(Point puntoExtremo1, Point puntoExtremo2) = IntersectLine(ray);
		
		return ( Math.Sign(GetLambda(puntoExtremo1)) == Math.Sign(GetLambda(ray.Point2))) ? puntoExtremo1 : puntoExtremo2;

		double GetLambda(Point pointX)
		{
			float lambda = 0;
			if(vector.X != 0)
				lambda = (pointX.X - ray.Point1.X)/ vector.X;	
			else 
				lambda = (pointX.Y - ray.Point1.Y)/ vector.Y;

			return lambda;
		}
	}

	private Point ArcMiddlePoint(Arc arc)
	{
		float middleAngle = (arc.StarAngle + arc.EndAngle)/2;

		return new Point(arc.Radio * MathF.Cos(middleAngle) , arc.Radio * MathF.Sin(middleAngle) );
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
