﻿using UnityEngine;

public class FOV
{
    int mapWidth = 0;
    int mapHeight = 0;

    public void Init()
    {
        mapWidth = TileManager.Instance.mapWidth;
        mapHeight = TileManager.Instance.mapHeight;
    }

    private float degreeToRadian(int _degree)
    {
        return _degree * (Mathf.PI / 180);
    }
    
    private float distance(float _a, float _b)
    {
        return _a > _b ? _a : _b;
    }

    private float diagonalDistance(int _x0, int _y0, int _x1, int _y1)
    {
        int dx = _x1 - _x0;
        int dy = _y1 - _y0;

        return distance(Mathf.Abs(dx), Mathf.Abs(dy));
    }

    public void CalcFov(Tile[,] _tilemap, int _x, int _y, int _distance)
    {
        
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                if (!_tilemap[j, i].tileData.isSeen) continue; //아직 보지 못 했으면 무시

                _tilemap[j, i].tileData.isSighted = false;
                _tilemap[j, i].FOV_spriteRenderer.color = new Color(255, 255, 255, 0.5f);
            }
        }      

        for (int i = 0; i < 360; i++) //1도씩 360도 계산
        {
            float degree = degreeToRadian(i);

            int nx = Mathf.RoundToInt(Mathf.Cos(degree) * _distance) + _x;
            int ny = Mathf.RoundToInt(Mathf.Sin(degree) * _distance) + _y;

            float distance = diagonalDistance(_x, _y, nx, ny); //각도당 시야 거리 계산

            for(int j = 0; j < (int)distance; j++) 
            {
                int tileX = Mathf.RoundToInt(Mathf.Lerp(_x, nx, j / distance)); //러프를 이용해서 걸리는 타일을 뽑는다.
                int tileY = Mathf.RoundToInt(Mathf.Lerp(_y, ny, j / distance));               

                if (tileX < 0 || tileX >= mapWidth) continue;
                if (tileY < 0 || tileY >= mapHeight) continue;

                if(_tilemap[tileX,tileY].tileData.tileRestriction == TILE_RESTRICTION.FORBIDDEN) //벽을 만나면
                {
                    if(_tilemap[tileX, tileY].tileData.isSeen != true) _tilemap[tileX, tileY].tileData.isSeen = true;
                    _tilemap[tileX, tileY].tileData.isSighted = true;
                    _tilemap[tileX,tileY].FOV_spriteRenderer.color = new Color(255, 255, 255, 0f);
                    break; //그 뒤로는 검색 중지
                }
                else
                {           
                    if (_tilemap[tileX, tileY].tileData.isSeen != true) _tilemap[tileX, tileY].tileData.isSeen = true;
                    _tilemap[tileX, tileY].tileData.isSighted = true;
                    _tilemap[tileX, tileY].FOV_spriteRenderer.color = new Color(255, 255, 255, 0f);                                
                }
            }
        }      
    }
}