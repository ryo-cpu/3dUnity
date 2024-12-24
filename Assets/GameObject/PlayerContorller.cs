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

    private Rigidbody rb; // Rigidbody�R���|�[�l���g
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
        rb = GetComponent<Rigidbody>(); // Rigidbody�̎擾
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            // ��e�������}�e���A����ݒ�
            collider.material = nonElasticMaterial;
        }
        if (Renderer == null)
        {
            Renderer = GetComponent<Renderer>(); // �v���C���[�ɃA�^�b�`���ꂽRenderer���擾
        }

        if (Renderer == null)
        {
            Debug.LogError("Renderer �R���|�[�l���g��������܂���B");
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
                // �f�o�b�O�\��
                Vector3 localNormal = transform.InverseTransformDirection(normal);

              

                // �ڐG�_���V�[���r���[�ɕ`��
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
      
            Debug.Log("�ڐG���I�����܂����I");
            // �ڐG���Ȃ��Ȃ����Ƃ��̏����������ɋL�q
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
                // �ڐG�_�i�Ώۂ̕\�ʏ�̓_�j
                Vector3 contactPoint = hit.point;

                // �@���x�N�g��
                Vector3 normal = hit.normal;
                Forwad = normal;

                // �V�[���r���[�ɐ�����`��
                Debug.DrawLine(transform.position, contactPoint, Color.red); // ��������ڐG�_
                Debug.DrawRay(contactPoint, normal, Color.green); // �ڐG�_����@��
                Debug.Log("�I�u�W�F�N�gA�B");
            }
            else if(Physics.Raycast(transform.position, (-gameObject.transform.forward).normalized, out RaycastHit hit2, 200000f))
                {
                {
                
                    // �ڐG�_�i�Ώۂ̕\�ʏ�̓_�j
                    Vector3 contactPoint = hit2.point;

                    // �@���x�N�g��
                    Vector3 normal = hit2.normal;
                    Forwad = normal;

                    // �V�[���r���[�ɐ�����`��
                    Debug.DrawLine(transform.position, contactPoint, Color.red); // ��������ڐG�_
                    Debug.DrawRay(contactPoint, normal, Color.green); // �ڐG�_����@��
                    Debug.Log("�I�u�W�F�N�gB�B");
                }

               

            }
          else if (Physics.Raycast(transform.position, (gameObject.transform.right).normalized, out RaycastHit hit3, 200000f))
            {
                // �ڐG�_�i�Ώۂ̕\�ʏ�̓_�j
                Vector3 contactPoint = hit3.point;

                // �@���x�N�g��
                Vector3 normal = hit3.normal;
                Forwad = normal;

                // �V�[���r���[�ɐ�����`��
                Debug.DrawLine(transform.position, contactPoint, Color.red); // ��������ڐG�_
                Debug.DrawRay(contactPoint, normal, Color.green); // �ڐG�_����@��
                Debug.Log("�I�u�W�F�N�gC�B");
            }
            else if(Physics.Raycast(transform.position, (-gameObject.transform.right).normalized, out RaycastHit hit4, 200000f))
            {
                
                {
                    // �ڐG�_�i�Ώۂ̕\�ʏ�̓_�j
                    Vector3 contactPoint = hit4.point;

                    // �@���x�N�g��
                    Vector3 normal = hit4.normal;
                    Forwad = normal;

                    // �V�[���r���[�ɐ�����`��
                    Debug.DrawLine(transform.position, contactPoint, Color.red); // ��������ڐG�_
                    Debug.DrawRay(contactPoint, normal, Color.green); // �ڐG�_����@��
                    Debug.Log("�I�u�W�F�N�gD�B");
                }

               

            }
            else
            {
                Forwad= gameObject.transform.up;
                Debug.Log("�I�u�W�F�N�gVVVVVV�B");
            }
            if (gameObject != null)
            {
                Vector3 arbitraryVector = new Vector3(0, 1, 0); // �K���ȃx�N�g��
                Vector3 arbitraryVector2 = new Vector3(1, 0, 0); // �K���ȃx�N�g��


                // �@���x�N�g���ƔC�ӂ̃x�N�g���̊O�ς��v�Z
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
                    // ���͂��Ȃ���Ό���
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
                Debug.Log("�I�u�W�F�N�g��������܂���ł����B");
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
                // ���͂��Ȃ���Ό���
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
        // �����_���[�̃}�e���A�����擾���ĐF�̓����x��ύX
        if (Renderer != null)
        {
            Material mat = Renderer.material;
            mat.SetFloat("_Mpde", 3);
            var color = Renderer.material.color;
            color.a = alpha; // �����x��ݒ�
            mat.color = color; // �}�e���A���ɐV�����F��K�p
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000; // �������̗p�̃����_�����O����
        }
    }
}
