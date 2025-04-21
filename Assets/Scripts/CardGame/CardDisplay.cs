using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData; //ī�� ������
    public int cardIndex;

    public MeshRenderer cardRenderer; //ī�� ������
    public TextMeshPro nameText; //�̸� �ؽ�Ʈ
    public TextMeshPro costText; //��� �ؽ�Ʈ
    public TextMeshPro attackText; //���ݷ�/ȿ�� �ؽ�Ʈ
    public TextMeshPro descriptionText; //���� �ؽ�Ʈ

    public bool isDragging = false;
    private Vector3 originalPosition;

    public LayerMask enemyLayer; //�� ���̾�
    public LayerMask playerLayer; //�÷��̾� ���̾�

    private CardManager cardManager;
    // Start is called before the first frame update
    void Start()
    {
        //���̾� ����ũ ����
        playerLayer = LayerMask.GetMask("Player");
        enemyLayer = LayerMask.GetMask("Enemy");

        cardManager = FindObjectOfType<CardManager>();

        SetupCard(cardData);
    }

    public void SetupCard(CardData data)
    {
        cardData = data;

        if(nameText != null) nameText.text = data.cardName;
        if(costText != null) costText.text = data.manaCost.ToString();
        if (attackText != null) attackText.text = data.effectAmount.ToString();
        if (descriptionText != null) descriptionText.text = data.description;

        if(cardRenderer  != null&& data.artwork != null)
        {
            Material cardMaterial = cardRenderer.material;
            cardMaterial.mainTexture = data.artwork.texture;
        }
    }
    private void OnMouseDown()
    {
        originalPosition = transform.position;
        isDragging = true;
    }
    private void OnMouseDrag()
    {
        if(isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        }
    }
    
    private void OnMouseUp()
    {
        CaracterStats playerStats = FindAnyObjectByType<CaracterStats>();
        if(playerStats == null || playerStats.currentMana < cardData.manaCost)
        {
            Debug.Log($"������ �����մϴ�! (�ʿ� : {cardData.manaCost}, ���� : {playerStats?.currentMana ?? 0}");
            transform.position = originalPosition;
            return;
        }
        isDragging = false;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool cardUsed = false;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
        {
            CaracterStats enemyStats = hit.collider.GetComponent<CaracterStats>();
            if(enemyStats != null)
            {
                if(cardData.cardType == CardData.CardType.Attack)
                {
                    enemyStats.TakeDamage(cardData.effectAmount);
                    Debug.Log($"{cardData.cardName} ī��� ������ {cardData.effectAmount} �������� �������ϴ�.");
                    cardUsed = true;
                }
            }
        }
        else if(Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer))
        {
            //CaracterStats playerStats = hit.collider.GetComponent<CaracterStats>();
            if(playerStats != null)
            {
                if(cardData.cardType == CardData.CardType.Heal)
                {
                    playerStats.Heal(cardData.effectAmount);
                    Debug.Log($"{cardData.cardName} ī��� �÷��̾��� ü���� {cardData.effectAmount} ȸ���߽��ϴ�.");
                    cardUsed = true;
                }
            }
        }
        else if(cardManager != null)
        {
            float distToDiscard = Vector3.Distance(transform.position, cardManager.discardPosition.position);
            if(distToDiscard < 2.0f)
            {
                cardManager.DiscardCard(cardIndex);
                return;
            }
        }
        //���� ī�� ���� ��ó�� ��� �ߴ��� �˻�
        if (!cardUsed)
        {
            transform.position = originalPosition;

            cardManager.ArrangeHand();
        }
        else
        {
            if (cardManager != null)
                cardManager.DiscardCard(cardIndex);
            playerStats.UseMana(cardData.manaCost);
            Debug.Log($"������ {cardData.manaCost} ��� �߽��ϴ�. (���� ���� : {playerStats.currentMana}");
        }
    }
}
