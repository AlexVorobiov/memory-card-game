using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class CardFactory
{
    private readonly DiContainer _container;
    private readonly GameObject _cardPrefab;
    private readonly Transform _cardParent;
    private readonly List<Sprite> _cardSprites;

    public CardFactory(DiContainer container, GameObject cardPrefab, Transform cardParent, Sprite[] cardSprites)
    {
        _container = container;
        _cardPrefab = cardPrefab;
        _cardParent = cardParent;
        _cardSprites = cardSprites.ToList();
    }

    public List<Card> CreateCards(int amount)
    {
        var neededSprites = amount / 2;

        var cardList = new List<Card>();
        
        var selectedSprites = GetRandomSublist(_cardSprites, neededSprites);
        
        var cardSpriteList = new List<Tuple<int, Sprite>>();
        var i = 0;
        selectedSprites.ForEach(s =>
        {
            cardSpriteList.Add(new Tuple<int, Sprite>(i,s));
            i++;
        });
        
        cardSpriteList.AddRange(cardSpriteList);
        
        Shuffle(cardSpriteList);
        
        foreach (var sprite in cardSpriteList)
        {
            var cardObject = _container.InstantiatePrefabForComponent<Card>(_cardPrefab, _cardParent);
            cardObject.SetFrontSprite(sprite.Item2);
            cardObject.CardId = sprite.Item1;
            cardObject.isFlipped.Value = false;
            cardList.Add(cardObject);
        }

        return cardList;
    }
    
    private void Shuffle<T>(List<T> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var temp = list[i];
            var randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    
    public static List<T> GetRandomSublist<T>(List<T> list, int sublistSize)
    {
        if (sublistSize > list.Count)
            throw new ArgumentException("Sublist size cannot be greater than the original list size.");

        HashSet<int> selectedIndices = new HashSet<int>();
        List<T> sublist = new List<T>();
        var rnd = new System.Random();

        while (selectedIndices.Count < sublistSize)
        {
            int index = rnd.Next(list.Count);
            if (selectedIndices.Add(index))
            {
                sublist.Add(list[index]);
            }
        }

        return sublist;
    }
}