using System;
using System.Linq;
using JetBrains.Annotations;
using TileMap;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Test
{
    public class Core : TileCollision
    {
        [Tooltip("Гравитация в пикселях в секунду")]
        public Vector3 pixelGravity;

        [Tooltip("Скорость в пикселях в секунду")]
        public float pixelSpeed;

        protected Tilemap Tm;
        protected HorizontalDirection HDirection;


        public Vector3 Velocity;
        [CanBeNull] protected TileBase TmpTile;
        protected Bounds FakeBounds;

        protected bool IsGrounded;

        // TODO Сделать: если гравитация == 0.0f, то при обнаружении пустоты на нижней ячейке по горизонтали менять направление движения.
        protected void Start()
        {
            // Tm = MainController.Instance.tilemap;
            Velocity = Vector3.zero;
            FakeBounds = GetComponent<BoxCollider2D>().bounds;
        }

        // public float maxVerticalVelocity = 0f;
        // public float minVerticalVelocity = 0f;
        private bool isJumping = false;
        private bool isFullJump = true;

        protected void Jump(float minJumpHeight, float maxJumpHeight)
        {
            // Проверяем, нажата ли клавиша прыжка (например, Space) и персонаж не находится в состоянии прыжка
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                isFullJump = true;
                isJumping = true; // устанавливаем флаг прыжка
                Velocity.y =
                    Mathf.Sqrt(2 * pixelGravity.y *
                               maxJumpHeight); // устанавливаем начальную вертикальную скорость для максимального прыжка
                // minVerticalVelocity =
                //     Mathf.Sqrt(2 * pixelGravity.y *
                //                minJumpHeight); // устанавливаем начальную вертикальную скорость для минимального прыжка
            }

            if (Velocity.y == 0.0f) return;
            

            if (!isJumping)
            {
                Velocity.y = -pixelGravity.y * Time.deltaTime;
                // minVerticalVelocity = -pixelGravity.y * Time.deltaTime;
            }
            // else
            // {
            //     maxVerticalVelocity -= pixelGravity.y * Time.deltaTime;
            //     minVerticalVelocity -= pixelGravity.y * Time.deltaTime;
            // }


            // Обновляем позицию персонажа по вертикали

            // if (Input.GetKeyUp(KeyCode.Space) && isJumping && minVerticalVelocity > 0.0f)
            // {
            //     isFullJump = false;
            // }

            // if (isFullJump)
            // {
                transform.position += new Vector3(0f, Velocity.y * Time.deltaTime, 0f);
            // }
            // else
            // {
            //     transform.position += new Vector3(0f, minVerticalVelocity * Time.deltaTime, 0f);
            // }
        }

        protected void LastUpdateFallDown()
        {
            var pos = transform.position;
            var cell = Tm.WorldToCell(pos);
            var bottomTile = CenterBottomTile(Tm, FakeBounds);
            if (bottomTile is not null)
            {
                var cellPos = Tm.CellToWorld(cell);
                pos.y = cellPos.y + FakeBounds.extents.y;
                transform.position = pos;
                Velocity = Vector3.zero;
                isJumping = false; // сбрасываем флаг прыжка
                // maxVerticalVelocity = 0f; // обнуляем вертикальную скорость
                // minVerticalVelocity = 0f; // обнуляем вертикальную скорость
            }
            else
            {
                // if (!isJumping)
                // {
                    Velocity -= pixelGravity * Time.deltaTime;
                // }
            }

            transform.position += Velocity * Time.deltaTime;
        }


        protected void LastUpdateMoveHorizontal()
        {
            var pos = transform.position;
            FakeBounds.center = pos;
            var cell = Tm.WorldToCell(pos);
            switch (HDirection)
            {
                case HorizontalDirection.Left:
                    TmpTile = CenterLeftTile(Tm, FakeBounds);
                    // Debug.Log("LEFT");
                    if (TmpTile is not null)
                    {
                        var cellPos = Tm.CellToWorld(cell);
                        if (TmpTile.name.Contains("blo", StringComparison.OrdinalIgnoreCase))
                        {
                            Debug.Log(TmpTile.GetHashCode());
                            Tm.SetTile(cell, null);
                            Tm.RefreshTile(cell);
                            break;
                        }

                        HDirection = HorizontalDirection.Right;
                        pos.x = cellPos.x + FakeBounds.extents.x;
                    }

                    break;
                case HorizontalDirection.Right:
                    TmpTile = CenterRightTile(Tm, FakeBounds);
                    if (TmpTile is not null)
                    {
                        HDirection = HorizontalDirection.Left;
                        var cellPos = Tm.CellToWorld(cell);
                        pos.x = cellPos.x + Tm.cellSize.x -
                                FakeBounds.extents.x; //  херь - надо шоб по другому работало...
                    }

                    break;
                case HorizontalDirection.Stay:
                    pos.x = Mathf.Round(pos.x);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transform.position = pos;
        }

        protected void UpdateMoveHorizontal()
        {
            var speed = pixelSpeed * Time.deltaTime;
            switch (HDirection)
            {
                case HorizontalDirection.Left:
                    transform.position += Vector3.left * speed;
                    break;
                case HorizontalDirection.Right:
                    transform.position += Vector3.right * speed;
                    break;
                case HorizontalDirection.Stay:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}