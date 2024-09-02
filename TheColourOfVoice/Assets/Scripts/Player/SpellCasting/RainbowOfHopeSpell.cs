using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RainbowOfHopeSpell : Spell
{
    public float multiplier;
    public BulletBase bulletPrefab;
    
    public Fire fire;

    private BulletBase bulletCache;
    
    private int index;
    
    public static Dictionary<char, Sprite> charToSprite;

    public override void SetUp()
    {
        base.SetUp();
        if (charToSprite == null)
        {
            charToSprite = new Dictionary<char, Sprite>();
            for (int i = 33; i < 91; i++)
            {
                char c = (char) i;
                Sprite sprite = Resources.Load<Sprite>(PathDefines.CharPath + i);
                charToSprite.Add(c, sprite);
            }
        }
    }
    
    public override void Execute()
    {
        base.Execute();
        fire.SetInterval(fire.shootingInterval / multiplier);
        bulletCache = fire.bulletPrefab;
        fire.SetBullet(bulletPrefab);
        fire.onFire += OnFire;
    }

    void OnFire(BulletBase bullet)
    {
        if (bullet.TryGetComponent(out SpriteRenderer sp))
        {
            while (index < triggerWords.Length){

                char ch = triggerWords[index++];
                if (ch >= 97 && ch <= 122)
                {
                    ch = (char) (ch - 32);
                }
                if (ch != ' ')
                {
                    sp.sprite = charToSprite[ch];
                    return;
                }
            }
            Finish();
        }
    }
    
    void Finish()
    {
        fire.SetInterval(fire.shootingInterval * multiplier);
        fire.SetBullet(bulletCache);
        fire.onFire -= OnFire;
        index = 0;
    }
}