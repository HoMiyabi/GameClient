// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class HeroController : MonoBehaviour
// {
//     //根据标签player获取英雄对象
//     private GameObject hero;
//
//
//     public float speed = 2; //每秒移动的距离
//
//     //是否调整视角
//     public bool AdjustView{get; set;}
//     //是否调整距离
//     public bool AdjustDistance{get; set;}
//
//     private Camera camera;
//
//
//     //记录摄像机相对位置
//     Vector3 offset;
//
//
//     // HeroAnimations anim;
//
//     void Start()
//     {
//         camera = Camera.main;
//         //根据标签player获取英雄对象
//         hero = this.gameObject;
//         //摄像机移动到英雄后方且对准英雄
//         camera.transform.position = hero.transform.position - hero.transform.forward * 8 + Vector3.up * 3;
//         camera.transform.LookAt(hero.transform);
//         offset = camera.transform.position - hero.transform.position;
//
//         // anim = GetComponent<HeroAnimations>();
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//
//         // if(Input.GetKeyDown(KeyCode.Space))
//         // {
//         //     anim.PlayAttack1();
//         // }
//
//         //摄像机跟随英雄移动
//         camera.transform.position = hero.transform.position + offset;
//
//         //鼠标滚轮控制摄像机距离英雄的距离
//         float wheel = Input.GetAxis("Mouse ScrollWheel");
//         if (wheel != 0)
//         {
//             camera.transform.position += camera.transform.forward * 1.5f * wheel;
//             offset = camera.transform.position - hero.transform.position;
//         }
//
//         //鼠标右键控制摄像机绕英雄旋转
//         if (Input.GetMouseButton(1))
//         {
//             //Debug.Log("Mouse 0");
//             float x = Input.GetAxis("Mouse X");
//             float y = Input.GetAxis("Mouse Y");
//             camera.transform.RotateAround(hero.transform.position, Vector3.up, x * 2);
//             camera.transform.RotateAround(hero.transform.position, camera.transform.right, -y * 2);
//             offset = camera.transform.position - hero.transform.position;
//         }
//
//         //offset最大距离为20
//         offset = Vector3.ClampMagnitude(offset, 20);
//
//
//         //控制英雄移动
//         float h = 0;
//         float v = 0;
//         if(h==0)h=Input.GetAxis("Horizontal");
//         if(v==0)v=Input.GetAxis("Vertical");
//         if (h != 0 || v != 0)
//         {
//             //播放跑步动画
//             // anim.PlayRun();
//             // if (anim.state == HeroAnimations.HState.Run)
//             // {
//             //     //摇杆控制英雄沿着摄像机的方向移动
//             //     Vector3 dir = camera.transform.forward * v + camera.transform.right * h;
//             //     dir.y = 0;
//             //     dir.Normalize();
//             //     hero.transform.position += dir * speed * Time.deltaTime;
//             //     hero.transform.forward = dir;
//             // }
//         }
//         else
//         {
//             //播放待机动画
//             // anim.PlayIdle();
//         }
//
//
//         //用射线检测摄像机与英雄之间是否有障碍物
//         RaycastHit hit;
//         // if (Physics.Linecast(hero.transform.position, camera.transform.position, out hit))
//         // {
//         //     //临时移动摄像机到障碍物的位置
//         //     camera.transform.position = hit.point;
//         // }
//
//         //射线检测摄像机不能穿过地面
//         if (Physics.Linecast(hero.transform.position+Vector3.up*0.5f, camera.transform.position-Vector3.up*0.3f, out hit))
//         {
//             //临时移动摄像机到障碍物的位置
//             camera.transform.position = hit.point + Vector3.up * 0.5f;
//         }
//
//     }
//
//
// }
