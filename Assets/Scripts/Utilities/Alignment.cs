using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Alignment
{
    public static void Grid(Component[] transforms, Vector3 localOffset)
    {
        float padding = 1.0f;
        float downOffset = 8.0f;
        int rows = Mathf.CeilToInt((float)transforms.Length / 4.0f);
        if (rows == 1 && transforms.Length == 4)
        {
            ++rows;
        }
        int columns = Mathf.CeilToInt((float)transforms.Length / (float)rows);

        localOffset += Vector3.back * ((float)Mathf.Max(0, rows - 1) * ((10.0f + padding) / 2.0f) - downOffset);
        localOffset += Vector3.right * ((float)Mathf.Max(0, columns - 1) * ((10.0f + padding) / 2.0f));

        int rowsOffsetted = (rows * columns) - transforms.Length;
        int index = 0;

        for (int i = 0; i < rows; ++i)
        {
            int localColumns = columns;
            Vector3 offset = Vector3.zero;

            if (rowsOffsetted > 0)
            {
                --rowsOffsetted;
                offset = Vector3.left * 5.0f;
                --localColumns;
            }

            for (int j = 0; j < localColumns; ++j)
            {
                transforms[index++].transform.localPosition = localOffset + offset + Vector3.left * (10.0f + padding) * j + Vector3.forward * i * (10.0f + padding);
                if (index == transforms.Length)
                {
                    break;
                }
            }
        }
    }
}
