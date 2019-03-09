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
    private bool abaixado;
    public bool NoChaoBool;

    void Start()
    {
        abaixado = false;
        AnimaBool("Abaixar", abaixado);
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            AnimaBool("NoChao", false);
            NoChaoBool = false;
            CalculoMovimentoAng();

            //Pulo
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDir.y = puloForca;
            }
        }
        else
        {
            AnimaBool("NoChao", true);
            NoChaoBool = true;
        }

        moveDir.y -= gravidade * Time.deltaTime;
        controller.Move(moveDir);
    }

    /*private void MovimentoPersonagem()
    {
        //Moviemnto do Eixo Z +
        if(Input.GetAxis("Vertical") > 0)
        {
            AnimaBool("direita", false);
            AnimaBool("esquerda", false);
            AnimaBool("diagonalDireita", false);
            AnimaBool("diagonalEsquerda", false);
            AnimaBool("correrB", false);
            AnimaBool("correr", true);
        }
        
        //Moviemnto do Eixo Z -
        if (Input.GetAxis("Vertical") < 0)
        {
            AnimaBool("direita", false);
            AnimaBool("esquerda", false);
            AnimaBool("diagonalEsquerdaT", false);
            AnimaBool("diagonalDireitaT", false);
            AnimaBool("correr", false);
            AnimaBool("correrB", true);
        }

        //Moviemnto do Eixo X +
        if (Input.GetAxis("Horizontal") > 0)
        {
            AnimaBool("correrB", false);
            AnimaBool("correr", false);
            AnimaBool("diagonalDireita", false);
            AnimaBool("diagonalEsquerda", false);
            AnimaBool("diagonalEsquerdaT", false);
            AnimaBool("diagonalDireitaT", false);
            AnimaBool("direita", true);
        }

        //Moviemnto do Eixo X -
        if (Input.GetAxis("Horizontal") < 0)
        {
            AnimaBool("correrB", false);
            AnimaBool("correr", false);
            AnimaBool("diagonalEsquerdaT", false);
            AnimaBool("diagonalDireitaT", false);
            AnimaBool("diagonalDireita", false);
            AnimaBool("diagonalEsquerda", false);
            AnimaBool("esquerda", true);
        }

        //Movimento Diagonal Direita Frente
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
        {
            AnimaBool("correr", false);
            AnimaBool("direita", false);
            AnimaBool("diagonalDireita", true);
        }

        //Movimento Diagonal Esquerda Frente
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
        {
            AnimaBool("diagonalEsquerda", true);
            AnimaBool("correr", false);
            AnimaBool("esquerda", false);
        }

        //Movimento Diagonal Direita Trás
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
        {
            AnimaBool("correrB", false);
            AnimaBool("direita", false);
            AnimaBool("diagonalDireitaT", true);
        }

        //Movimento Diagonal Esquerda Trás
        else if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
        {
            AnimaBool("diagonalEsquerdaT", true);
            AnimaBool("correrB", false);
            AnimaBool("esquerda", false);
        }

        //Idle
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            AnimaBool("correrB", false);
            AnimaBool("correr", false);
            AnimaBool("direita", false);
            AnimaBool("esquerda", false);
            AnimaBool("diagonalDireita", false);
            AnimaBool("diagonalEsquerda", false);
            AnimaBool("diagonalEsquerdaT", false);
            AnimaBool("diagonalDireitaT", false);
        }
    }*/

    public void AnimaBool(string nome, bool gatilho)
    {
        animator.SetBool(nome, gatilho);
    }

    public void AnimaFloat(string nome, float gatilho, float dampTime, float deltaTime)
    {
        animator.SetFloat(nome, gatilho, dampTime, deltaTime);
    }

    private void DefineAnimacaoAbaixar()
    {
        abaixado = !abaixado;
        AnimaBool("Abaixar", abaixado);
    }

    private void CalculoMovimentoAng()
    {
        moveZ = Input.GetAxis("Vertical") * 2 * Time.deltaTime;
        moveX = Input.GetAxis("Horizontal") * 2 * Time.deltaTime;

        transform.Rotate(0, Input.GetAxis("Mouse X") * 10, 0 * Time.deltaTime);

        Vector3 frente = transform.forward * moveZ;
        Vector3 lado = transform.right * moveX;

        moveDir = frente + lado;
        
        AnimaFloat("InputVertical", Input.GetAxis("Vertical"), 0.1f, Time.deltaTime);
        AnimaFloat("InputHorizontal", Input.GetAxis("Horizontal"), 0.1f, Time.deltaTime);

        Corrida();
        VerificaAbaixado();
    }

    private void VerificaAbaixado()
    {
        if (abaixado)
        {
            moveDir *= 0.5f;
            controller.height = 1.26f;
            controller.center = new Vector3(0, 0.73f, 0);
        }
        else
        {
            moveDir *= 2f;
            controller.height = 2.15f;
            controller.center = new Vector3(0, 1.12f, 0);
        }
    }

    private void Corrida()
    {
        if(moveZ > 0)
        {
            AnimaBool("MovimentoZ", true);
        }
        else
        {
            AnimaBool("MovimentoZ", false);
        }
        if(Input.GetKey(KeyCode.LeftShift) && !abaixado)
        {
            AnimaFloat("InputVertical", 1.5f, 0.1f, Time.deltaTime * 10);
            moveDir *= 2.1f;
        }
    }
}