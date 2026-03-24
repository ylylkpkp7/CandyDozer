using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    const int MaxShotPower = 5;
    const int RecoverySeconds = 3;

    int shotPower = MaxShotPower;

    public GameObject[] candyPrefabs;
    public Transform candyParentTransform;
    public CandyManager candyManager;
    public float shotForce;
    public float shotTorque;
    public float baseWidth;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shot();
        }
    }

    // ランダムでキャンディ選ぶ
    GameObject SampleCandy()
    {
        int index = Random.Range(0, candyPrefabs.Length); // ← Lengthに修正
        return candyPrefabs[index];
    }

    // 生成位置を計算
    Vector3 GetInstantiatePosition()
    {
        float mouseX = Mouse.current.position.ReadValue().x;

        float x = baseWidth * (mouseX / Screen.width - 0.5f);

        return transform.position + new Vector3(x, 0, 0);
    }

    public void Shot()
    {
        //キャンディを生成できる条件外ならShotしない
        if (candyManager.GetCandyAmount() <= 0) return;
        if (shotPower <= 0) return;

        //プレハブからCandyオブジェクトを生成
        GameObject candy = Instantiate(
            SampleCandy(), // ← 配列じゃなく1個選ぶ
            GetInstantiatePosition(), // ← 位置も関数使う
            Quaternion.identity,
            candyParentTransform // ← 親にセット
        );

        Rigidbody candyRigidBody = candy.GetComponent<Rigidbody>();

        if (candyRigidBody != null)
        {
            candyRigidBody.AddForce(transform.forward * shotForce);
            candyRigidBody.AddTorque(new Vector3(0, shotTorque, 0));
        }

        //Candyのストックを消費
        candyManager.ConsumeCandy();
        //ShotPowerを消費
        ConsumePower();
    }

    void OnGUI()
    {
        GUI.color = Color.black;

        //ShotPowerの残数を+の数で表示
        string label = "";
        for (int i = 0; i < shotPower; i++) label = label+ "+";

        GUI.Label(new Rect(50, 65, 100, 30), label);
    }
    void ConsumePower()
    {
        //ShotPowerを消費すると同時に回復のカウントをスタート
        shotPower--;
    }
}