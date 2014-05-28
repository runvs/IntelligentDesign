using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using JamUtilities.ScreenEffects;

namespace JamUtilities
{
    public class Animation
    {
        System.Collections.Generic.List<SmartSprite> _spriteList;

        public float SingleImageTime {get; set;}
        private float _currentTime;
        private int _currentPosition;
        public System.Collections.Generic.List<int> _order;

        public Animation(string fileName, IntRect spriteSize, uint numberOfSprites, float singleImageTime = 0.3f)
        {
            _spriteList = new List<SmartSprite>();
            _order = new List<int>();
            SingleImageTime = singleImageTime;
            _currentTime = singleImageTime;
            _currentPosition = 0;
            SFML.Graphics.Texture text = JamUtilities.TextureManager.GetTextureFromFileName(fileName);
            for(int i = 0; i < numberOfSprites; i++)
            {
                //System.Console.WriteLine(i);
                IntRect rect = new IntRect(i * spriteSize.Width, 0, spriteSize.Width,  spriteSize.Height);
                SmartSprite spr = new SmartSprite(text,rect);
                _spriteList.Add(spr);
                _order.Add(i);
            }

        }

        public void Update (TimeObject timeObject)
        {
            _currentTime -= timeObject.ElapsedGameTime;
            if (_currentTime <= 0.0f)
            {
                _currentTime = SingleImageTime;
                _currentPosition++;
                if (_currentPosition == _order.Count)
                {
                    _currentPosition = 0;
                }
            }
        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            //_currentPosition = _spriteList.Count - 1;
            _spriteList[_order[_currentPosition]].Draw(rw);
        }


        public Vector2f Position { get { return _spriteList[_order[_currentPosition]].Position - (ScreenEffects.ScreenEffects.GlobalSpriteOffset + ScreenEffects.ScreenEffects._dragPosition); } set { foreach (var _sprite in _spriteList) { _sprite.Position = value + (ScreenEffects.ScreenEffects.GlobalSpriteOffset + ScreenEffects.ScreenEffects._dragPosition); } } }
        public float Rotation { get { return _spriteList[_order[_currentPosition]].Rotation; } set { foreach (var _sprite in _spriteList) _sprite.Rotation = value; } }
        public Vector2f Origin { get { return _spriteList[_order[_currentPosition]].Origin; } set { foreach (var _sprite in _spriteList) _sprite.Origin = value; } }
        //public byte Alpha { get; set; }
        public Vector2f Scale { get { return _spriteList[_order[_currentPosition]].Sprite.Scale; }  }
    }
}
