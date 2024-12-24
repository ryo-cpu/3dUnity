using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerContorller : MonoBehaviour
{
    public float Speed;
    public float MaxSpeed = 20f;
    public float Deceleration = 0.1f;
    public float junp = 30f;
    [SerializeField] private Renderer Renderer;

    private Rigidbody rb; // Rigidbodyコンポーネント
    Vector3 JunpPower;
    Vector3 pos;
    Vector3 dir;
    bool isJunp;
    bool isDive;
    string DiveObject;
    Vector3 DivePoint;
    int Suraface;
    public PhysicMaterial nonElasticMaterial;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>(); // Rigidbodyの取得
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            // 非弾性物理マテリアルを設定
            collider.material = nonElasticMaterial;
        }
        if (Renderer == null)
        {
            Renderer = GetComponent<Renderer>(); // プレイヤーにアタッチされたRendererを取得
        }

        if (Renderer == null)
        {
            Debug.LogError("Renderer コンポーネントが見つかりません。");
        }
        dir = Vector3.zero;
        isJunp = false;
        isDive = false;

    }
    private void OnCollisionStay(Collision collision)
    {
       
        if (Input.GetKey(KeyCode.T))
        {
            dir = Vector3.zero;
            isDive = true;
            DiveObject = collision.gameObject.name;
            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 contactPoint = contact.point;
                Vector3 normal = contact.normal;
                // デバッグ表示
                Vector3 localNormal = transform.InverseTransformDirection(normal);

              

                // 接触点をシーンビューに描画
                Debug.DrawLine(contactPoint, contactPoint + Vector3.up * 0.5f, Color.red, 2f);
            }
            SetTransparency(0.6f);
        }
        else
        {
            isDive = false;
           
            DiveObject = null;
            SetTransparency(1f);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
      
            Debug.Log("接触が終了しました！");
            // 接触がなくなったときの処理をここに記述
            isDive = false;

            DiveObject = null;
            SetTransparency(1f);
        

    }
        // Update is called once per frame
        void Update()
    {
        pos = transform.position;
        Vector3 InputDir = Vector3.zero;
        if (isDive)
        {
           rb.isKinematic = false;
            GameObject gameObject=GameObject.Find(DiveObject);
            Vector3 Forwad;
            if(Physics.Raycast(transform.position, (gameObject.transform.forward).normalized, out RaycastHit hit,20000f))
        {
                // 接触点（対象の表面上の点）
                Vector3 contactPoint = hit.point;

                // 法線ベクトル
                Vector3 normal = hit.normal;
                Forwad = normal;

                // シーンビューに垂線を描画
                Debug.DrawLine(transform.position, contactPoint, Color.red); // 自分から接触点
                Debug.DrawRay(contactPoint, normal, Color.green); // 接触点から法線
                Debug.Log("オブジェクトA。");
            }
            else if(Physics.Raycast(transform.position, (-gameObject.transform.forward).normalized, out RaycastHit hit2, 200000f))
                {
                {
                
                    // 接触点（対象の表面上の点）
                    Vector3 contactPoint = hit2.point;

                    // 法線ベクトル
                    Vector3 normal = hit2.normal;
                    Forwad = normal;

                    // シーンビューに垂線を描画
                    Debug.DrawLine(transform.position, contactPoint, Color.red); // 自分から接触点
                    Debug.DrawRay(contactPoint, normal, Color.green); // 接触点から法線
                    Debug.Log("オブジェクトB。");
                }

               

            }
          else if (Physics.Raycast(transform.position, (gameObject.transform.right).normalized, out RaycastHit hit3, 200000f))
            {
                // 接触点（対象の表面上の点）
                Vector3 contactPoint = hit3.point;

                // 法線ベクトル
                Vector3 normal = hit3.normal;
                Forwad = normal;

                // シーンビューに垂線を描画
                Debug.DrawLine(transform.position, contactPoint, Color.red); // 自分から接触点
                Debug.DrawRay(contactPoint, normal, Color.green); // 接触点から法線
                Debug.Log("オブジェクトC。");
            }
            else if(Physics.Raycast(transform.position, (-gameObject.transform.right).normalized, out RaycastHit hit4, 200000f))
            {
                
                {
                    // 接触点（対象の表面上の点）
                    Vector3 contactPoint = hit4.point;

                    // 法線ベクトル
                    Vector3 normal = hit4.normal;
                    Forwad = normal;

                    // シーンビューに垂線を描画
                    Debug.DrawLine(transform.position, contactPoint, Color.red); // 自分から接触点
                    Debug.DrawRay(contactPoint, normal, Color.green); // 接触点から法線
                    Debug.Log("オブジェクトD。");
                }

               

            }
            else
            {
                Forwad= gameObject.transform.up;
                Debug.Log("オブジェクトVVVVVV。");
            }
            if (gameObject != null)
            {
                Vector3 arbitraryVector = new Vector3(0, 1, 0); // 適当なベクトル
                Vector3 arbitraryVector2 = new Vector3(1, 0, 0); // 適当なベクトル


                // 法線ベクトルと任意のベクトルの外積を計算
                Vector3 perpendicular = Vector3.Cross(Forwad, arbitraryVector);
                Vector3 perpendicular2 = Vector3.Cross(Forwad, arbitraryVector2);



                if (Input.GetKey(KeyCode.W))
                {
                    InputDir -= perpendicular2;
           
                }
                if (Input.GetKey(KeyCode.S))
                {
                    InputDir += perpendicular2;
                    

                }
                if (Input.GetKey(KeyCode.D))
                {
                    InputDir -= perpendicular;



                }
                if (Input.GetKey(KeyCode.A))
                {
                    InputDir +=perpendicular;
                }
                





                if (InputDir.magnitude > 0)
                {
                    dir += InputDir * Speed * Time.deltaTime;
                }
                else
                {
                    // 入力がなければ減速
                    dir = Vector3.Lerp(dir, Vector3.zero, 1 / (Deceleration / 60));
                }
                if (dir.magnitude > MaxSpeed)
                {
                    dir = dir.normalized * MaxSpeed;
                }

                dir.Normalize();
                transform.Translate(dir * Speed / 60);
            }
            else
            {
                Debug.Log("オブジェクトが見つかりませんでした。");
            }
        }
       
        else
        {
            rb.isKinematic = true;

            Vector3 verticalVector = transform.TransformDirection(Vector3.up);
            verticalVector = verticalVector.normalized;
            Vector3 anyOtherVector = Vector3.forward;
            Vector3 perpendicularVector = Vector3.Cross(verticalVector, anyOtherVector);
            perpendicularVector.Normalize();
            Vector3 anyOtherVector2 = Vector3.right;
            Vector3 perpendicularVector2 = Vector3.Cross(verticalVector, anyOtherVector2);
            perpendicularVector2.Normalize();
           
            if (Input.GetKey(KeyCode.W))
            {
                InputDir -=perpendicularVector2;
                
            }
            if (Input.GetKey(KeyCode.S))
            {
                InputDir += perpendicularVector2;


            }
            if (Input.GetKey(KeyCode.D))
            {

                InputDir += perpendicularVector;


            }
            if (Input.GetKey(KeyCode.A))
            {
                InputDir-= perpendicularVector;
            }
            if (Input.GetKey(KeyCode.Space) && !isJunp)
            {
                isJunp = true;
                JunpPower.y = junp;
            }
            if (isJunp)
            {
                dir += JunpPower;
                JunpPower.y--;
                JunpPower.y = JunpPower.y < 0 ? 0 : JunpPower.y;
                if (JunpPower.y < 0)
                {
                    isJunp = false;
                }

            }
            if (InputDir.magnitude > 0)
            {
                dir += InputDir * Speed * Time.deltaTime;
            }
            else
            {
                if (dir.magnitude > 0)
                { 
                // 入力がなければ減速
                dir = Vector3.Lerp(dir, Vector3.zero, 1 / (Deceleration / 60));
                }
            }
            if (dir.magnitude > MaxSpeed)
            {
                dir = dir.normalized * MaxSpeed;
            }

            dir.Normalize();
            transform.Translate(dir * Speed / 60);
        }

        


    }
    void SetTransparency(float alpha)
    {
        // レンダラーのマテリアルを取得して色の透明度を変更
        if (Renderer != null)
        {
            Material mat = Renderer.material;
            mat.SetFloat("_Mpde", 3);
            var color = Renderer.material.color;
            color.a = alpha; // 透明度を設定
            mat.color = color; // マテリアルに新しい色を適用
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000; // 透明物体用のレンダリング順序
        }
    }
}
