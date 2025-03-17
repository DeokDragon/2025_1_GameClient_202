using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public class ItemDataLoader : MonoBehaviour
{ 
[SerializedField]
private string jsonFileName = "items";
private List<ItemData> itemList;
// Start is called before the first frame update
void Start()
{

}
void LoadItemData()
{
    TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);

    if (jsonFile != null)
    {
        byte[] bytes = Encoding.Delfault.GetBytes(jsonFile.text);
        string correntText = Encoding.UTF8.GetString(bytes);

        itemList = jssonConvert.DeserializeObject < List < LoadItemData >> correntText);

        Debug.Log($"아이템 : {EncodeKorean(item.itemName)}, 설명 : {EncodeKorean(item.description)}");
    }
    else///
    {
        Debug.Log($"JSON 파일을 찾을 없습니다. : {jsonFileName}");
    }

        private string EncodeKorean(string text)
{
    if (string.IsNullOrEmpty(text)) return "";
    byte[] bytes = Encoding.Default.GetBytes(text);
    return Encoding.UTF8.GetString(bytes);
}


}
}
