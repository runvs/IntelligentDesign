using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace JamUtilities
{
    public class SmartSprite
    {
        public SmartSprite(string filepath)
        {
            _texture = TextureManager.GetTextureFromFileName(filepath);
            _sprite = new Sprite(_texture);
            _sprite.Scale = _scaleVector;
            IsInFade = false;
            Alpha = 255;
            Offset = new Vector2f(0.0f, 0.0f);
            GlobalOffsetFactor = 1.0f;
        }
        public SmartSprite(Texture texture)
        {
            _texture = texture;
            _sprite = new Sprite(_texture);
            _sprite.Scale = _scaleVector;
            IsInFade = false;
            Alpha = 255;
            Offset = new Vector2f(0.0f, 0.0f);
            GlobalOffsetFactor = 1.0f;
        }
        public SmartSprite(Texture texture, IntRect rect)
        {
            _texture = texture;
            _sprite = new Sprite(_texture, rect);
            _sprite.Scale = _scaleVector;
            IsInFade = false;
            Alpha = 255;
            Offset = new Vector2f(0.0f, 0.0f);
            GlobalOffsetFactor = 1.0f;
        }


        public void Flash(Color col, float duration)
        {

            if (duration < 0.0f)
            {
                throw new ArgumentOutOfRangeException("duration", duration, "Time for a flash must be non-negative");
            }
            IsInFlash = true;
            _timeSinceStartFlash = 0.0f;
            _totalTimeFlash = duration;
            _flashColor = col;
        }


        public void Fade(float duration)
        {
            if (duration < 0.0f)
            {
                throw new ArgumentOutOfRangeException("duration", duration, "Time for a fade must be non-negative");
            }

            IsInFade = true;
            _remainingTimeFade = duration;
            _totalTimeFade = duration;
        }

        public void Shake(float duration, float shakeTime, float power, ShakeDirection shakeDirection = ShakeDirection.AllDirections)
        {
            if (duration < 0.0f)
            {
                throw new ArgumentOutOfRangeException("duration", duration, "Duration for a shake must be non-negative");
            }
            if (shakeTime < 0.0f)
            {
                throw new ArgumentOutOfRangeException("shakeTime", shakeTime, "Time for a shake must be non-negative");
            }

            IsInShake = true;
            _remainingShakeTime = duration;
            _shakePower = power;
            _shakeTimer = 0.0f;
            _shakeTimerMax = shakeTime;
            _shakeDirection = shakeDirection;
        }

        // This is a either or question, calling it once with x and then with y will only do y scaling
        public void Scale(float factor, ShakeDirection dir = ShakeDirection.AllDirections)
        {
            if (dir == ShakeDirection.AllDirections)
            {
                _sprite.Scale = new Vector2f(factor * _scaleVector.X, factor * _scaleVector.X);
            }
            else if (dir == ShakeDirection.UpDown)
            {
                _sprite.Scale = new Vector2f(_scaleVector.X, factor * _scaleVector.X);
            }
            else if (dir == ShakeDirection.LeftRight)
            {
                _sprite.Scale = new Vector2f(factor * _scaleVector.X, _scaleVector.X);
            }
        }

        public void Scale(float factorX, float factorY)
        {
            _sprite.Scale = new Vector2f(factorX * _scaleVector.X, factorY * _scaleVector.X);

        }

        public void BreakPixels(float duration, List<Color> colorList, Vector2f direction, float initialPower)
        {
            _remainingTimeBrokenPixels = duration;
            IsInBrokenPixelMode = true;
            if (colorList.Count == 0)
            {
                throw new Exception("Color list may not be empty!");
            }
            _brokenPixelColorList = colorList;
            float length = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            if (length <= 1e-5 && length >= -1e-5)
            {
                throw new DivideByZeroException();
            }
            BrokenPixelDirection = direction / length;
            BrokenPixelPower = initialPower;

        }

        public void Update(TimeObject timeObject)
        {
            Update(timeObject.ElapsedGameTime);
        }

        public void Update(float deltaT)
        {
            if (IsInFlash)
            {
                _timeSinceStartFlash += deltaT;
                Color col = _sprite.Color;
                if (_timeSinceStartFlash < _totalTimeFlash)
                {
                    col = _flashColor;
                    double angle = Math.PI / _totalTimeFlash * _timeSinceStartFlash;
                    double sinAngle = Math.Sin(angle);
                    float r = (float)(col.R) * (1.0f * (2.0f - 0.5f * ((float)sinAngle + 1.0f)));
                    float g = (float)(col.G) * (1.0f * (2.0f - 0.5f * ((float)sinAngle + 1.0f)));
                    float b = (float)(col.B) * (1.0f * (2.0f - 0.5f * ((float)sinAngle + 1.0f)));
                    col.R = (byte)(r);
                    col.G = (byte)(g);
                    col.B = (byte)(b);
                }
                else
                {
                    col = Color.White;
                    IsInFlash = false;
                }
                _sprite.Color = col;
            }

            if (IsInFade)
            {
                _remainingTimeFade -= deltaT;
                Color col = _sprite.Color;
                if (_remainingTimeFade >= 0)
                {
                    col.A = (byte)(_remainingTimeFade / _totalTimeFade * 255.0f);
                }
                else
                {
                    col.A = 0;
                    IsInFade = false;
                }
                Alpha = col.A;
                //Console.WriteLine(col.A);
            }
            if (IsInShake)
            {
                _shakeTimer -= deltaT;
                if (_shakeTimer <= 0.0f)
                {
                    if (_shakeDirection == ShakeDirection.AllDirections)
                    {
                        Offset = RandomGenerator.GetRandomVector2fSquare(_shakePower);
                    }
                    else if (_shakeDirection == ShakeDirection.UpDown)
                    {
                        Offset = new Vector2f(0.0f, (float)(RandomGenerator.Random.NextDouble() - 0.5f) * 2.0f * _shakePower);
                    }
                    else if (_shakeDirection == ShakeDirection.UpDown)
                    {
                        Offset = new Vector2f((float)(RandomGenerator.Random.NextDouble() - 0.5f) * 2.0f * _shakePower, 0.0f);
                    }
                    _shakeTimer = _shakeTimerMax;
                }

                _remainingShakeTime -= deltaT;
                if (_remainingShakeTime <= 0.0f)
                {
                    Offset = new Vector2f(0.0f, 0.0f);
                    IsInShake = false;
                }

                _sprite.Position = Position;
            }
            if (IsInBrokenPixelMode)
            {
                _remainingTimeBrokenPixels -= deltaT;
                if (_remainingTimeBrokenPixels <= 0.0f)
                {
                    IsInBrokenPixelMode = false;
                }
            }
        }

        public void Draw(RenderWindow rw)
        {
            if (IsInBrokenPixelMode)
            {
                // getting the original values
                Color oldColor = _sprite.Color;
                Vector2f oldpos = _sprite.Position;


                float counter = 0.0f;
                float positionIncrement = BrokenPixelPower / (float)(_brokenPixelColorList.Count - 1);
                Vector2f positionStart = _sprite.Position - (0.5f * BrokenPixelDirection * BrokenPixelPower);
                foreach (var c in _brokenPixelColorList)
                {
                    _sprite.Position = positionStart + (positionIncrement * counter) * BrokenPixelDirection;
                    _sprite.Color = c;
                    rw.Draw(_sprite);
                    counter += 1.0f;
                }

                // resetting the original values
                _sprite.Color = oldColor;
                _sprite.Position = oldpos;
            }
            else
            {
                Color col = _sprite.Color;
                byte oldAlpha = col.A;
                col.A = Alpha;
                _sprite.Color = col;
                rw.Draw(_sprite);
                col.A = oldAlpha;
                _sprite.Color = col;
            }
        }


        #region FIELDS

        private Texture _texture;
        private Sprite _sprite;
        public static Vector2f _scaleVector;

        public Vector2f Position
        {
            get { return _sprite.Position - Offset - (ScreenEffects.ScreenEffects.GlobalSpriteOffset + ScreenEffects.ScreenEffects._dragPosition) * GlobalOffsetFactor; }
            set { _sprite.Position = value + Offset + (ScreenEffects.ScreenEffects.GlobalSpriteOffset + ScreenEffects.ScreenEffects._dragPosition) * GlobalOffsetFactor; }
        }
        public float Rotation { get { return _sprite.Rotation; } set { _sprite.Rotation = value; } }
        public Vector2f Origin { get { return _sprite.Origin; } set { _sprite.Origin = value; } }
        public byte Alpha { get; set; }
        public Vector2f Size
        {
            get
            {
                var rect = _sprite.GetGlobalBounds();
                return new Vector2f(rect.Width, rect.Width);
            }
        }


        public Vector2f Offset { get; private set; }
        public bool IsInShake { get; private set; }
        private float _remainingShakeTime;
        private float _shakeTimer;
        private float _shakeTimerMax;
        private float _shakePower;
        private ShakeDirection _shakeDirection;

        public Sprite Sprite { get { return _sprite; } }

        public bool IsInFade { get; private set; }
        private float _remainingTimeFade;
        private float _totalTimeFade;

        public bool IsInFlash { get; private set; }
        private float _timeSinceStartFlash;
        private float _totalTimeFlash;
        private Color _flashColor;

        public bool IsInBrokenPixelMode { get; private set; }
        private float _remainingTimeBrokenPixels;
        public Vector2f BrokenPixelDirection { get; private set; }
        public float BrokenPixelPower { get; set; }
        private List<Color> _brokenPixelColorList;

        // this Factor works on the global Sprite Offset Vector from the ScreenEffects class
        public float GlobalOffsetFactor { get; set; }

        #endregion FIELDS
    }
}
