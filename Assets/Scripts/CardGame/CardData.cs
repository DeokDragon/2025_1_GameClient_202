using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard" , menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    public enum CardType // Ä«µå Å¸ÀÔ ¿­°ÅÇü Ãß°¡
    {
        Attack,
        Heal,
        Buff,
        Utility
    }

    public string cardName;
    public string description;
    public Sprite artwork;
    public int manaCost;
    public int effectAmount;
    public CardType cardType;

    public Color GetCardColor()
    {
        switch(cardType)
        {
            case CardType.Attack:
                return new Color(0.9f, 0.3f, 0.3f); //»¡°£
            case CardType.Heal:
                return new Color(0.3f, 0.9f, 0.3f); //³ì»ö
            case CardType.Buff:
                return new Color(0.3f, 0.3f, 0.9f); //ÆÄ¶û
            case CardType.Utility:
                return new Color(0.9f, 0.9f, 0.3f); //³ë¶û
            default:
                return Color.white;
        }
    }
}
