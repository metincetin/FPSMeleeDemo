using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FPSMeleeDemo.UI
{

	public class Cursor : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<Cursor, UxmlTraits> { }

		private VisualElement _lines;

		private const float DefaultLineDistance = 24;
		private const float LineDissappearingDistance = 32;

		public Cursor()
		{
			Add(new VisualElement { name = "mid" });
			var lines = new VisualElement { name = "lines" };
			_lines = lines;

			Add(lines);


			for (int i = 0; i < 4; i++)
			{
				var line = new VisualElement();
				line.AddToClassList($"line");

				SetLineDistance(line, i, LineDissappearingDistance);

				lines.Add(line);
			}
		}

		public void ShowDamage()
		{
			var i = 0;
			foreach (var l in _lines.hierarchy.Children())
			{
				l.EnableInClassList("visible", true);

				SetLineDistance(l, i, DefaultLineDistance);

				i++;
			}
		}

		private void SetLineDistance(VisualElement line, int index, float distance)
		{
			float angle = (360 / 4f) * index + (360 / 4f / 2f);

			float angleRad = Mathf.Deg2Rad * angle;


			var dir = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

			line.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(dir.x, dir.y));
			line.style.rotate = new StyleRotate(new Rotate(angle));
			line.style.translate = new StyleTranslate(new Translate(dir.x * distance, dir.y * distance));

		}

		public void HideDamage()
		{
			var i = 0;
			foreach (var l in _lines.hierarchy.Children())
			{
				l.EnableInClassList("visible", false);

				SetLineDistance(l, i, LineDissappearingDistance);

				i++;
			}
		}
	}
}
