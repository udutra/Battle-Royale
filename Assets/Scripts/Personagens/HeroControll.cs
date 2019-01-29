using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControll : MonoBehaviour, IAnima
{
    public Vector3 moveDir = Vector3.zero;
    public CharacterController controller;
    public float velMove, gravidade, moveZ, moveX;
    public Animator animator;
    [SerializeField]
    public float puloForca;

    void Start()
    {
        
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            Movimento("caiu", false);
            CalculoMovimentoAng();

            //Pulo
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDir.y = puloForca;
            }
        }
        else
        {
            Movimento("caiu", true);
        }

        moveDir.y -= gravidade * Time.deltaTime;
        Console.WriteLine("MoveDir: " + moveDir);
        controller.Move(moveDir);
    }

    private void CalculoMovimentoAng()
    {
        moveZ = Input.GetAxis("Vertical") * 2 * Time.deltaTime;
        moveX = Input.GetAxis("Horizontal") * 2 * Time.deltaTime;

        transform.Rotate(0, Input.GetAxis("Mouse X") * 10, 0 * Time.deltaTime);

        Vector3 frente = transform.forward * moveZ;
        Vector3 lado = transform.right * moveX;

        moveDir = frente + lado;
        moveDir *= 2;

        MovimentoPersonagem();
    }

    private void MovimentoPersonagem()
    {
        //Moviemnto do Eixo Z +
        if(Input.GetAxis("Vertical") > 0)
        {
            Movimento("direita", false);
            Movimento("esquerda", false);
            Movimento("diagonalDireita", false);
            Movimento("diagonalEsquerda", false);
            Movimento("correrB", false);
            Movimento("correr", true);
        }
        
        //Moviemnto do Eixo Z -
        if (Input.GetAxis("Vertical") < 0)
        {
            Movimento("direita", false);
            Movimento("esquerda", false);
            Movimento("diagonalEsquerdaT", false);
            Movimento("diagonalDireitaT", false);
            Movimento("correr", false);
            Movimento("correrB", true);
        }

        //Moviemnto do Eixo X +
        if (Input.GetAxis("Horizontal") > 0)
        {
            Movimento("correrB", false);
            Movimento("correr", false);
            Movimento("diagonalDireita", false);
            Movimento("diagonalEsquerda", false);
            Movimento("diagonalEsquerdaT", false);
            Movimento("diagonalDireitaT", false);
            Movimento("direita", true);
        }

        //Moviemnto do Eixo X -
        if (Input.GetAxis("Horizontal") < 0)
        {
            Movimento("correrB", false);
            Movimento("correr", false);
            Movimento("diagonalEsquerdaT", false);
            Movimento("diagonalDireitaT", false);
            Movimento("diagonalDireita", false);
            Movimento("diagonalEsquerda", false);
            Movimento("esquerda", true);
        }

        //Movimento Diagonal Direita Frente
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
        {
            Movimento("correr", false);
            Movimento("direita", false);
            Movimento("diagonalDireita", true);
        }

        //Movimento Diagonal Esquerda Frente
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
        {
            Movimento("diagonalEsquerda", true);
            Movimento("correr", false);
            Movimento("esquerda", false);
        }

        //Movimento Diagonal Direita Trás
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
        {
            Movimento("correrB", false);
            Movimento("direita", false);
            Movimento("diagonalDireitaT", true);
        }

        //Movimento Diagonal Esquerda Trás
        else if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
        {
            Movimento("diagonalEsquerdaT", true);
            Movimento("correrB", false);
            Movimento("esquerda", false);
        }

        //Idle
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            Movimento("correrB", false);
            Movimento("correr", false);
            Movimento("direita", false);
            Movimento("esquerda", false);
            Movimento("diagonalDireita", false);
            Movimento("diagonalEsquerda", false);
            Movimento("diagonalEsquerdaT", false);
            Movimento("diagonalDireitaT", false);
        }
    }

    public void Movimento(string nome, bool gatilho)
    {
        animator.SetBool(nome, gatilho);
    }
}