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
    [SerializeField]
    private bool abaixado;
    public bool picareta;
    public GameObject[] picaretaGO;
    private bool ativaTempoReset;
    public float comboTempoPadrao = 0.4f;
    private float comboTempoAtual;
    public ComboPicareta comboEstado;

    void Start()
    {
        comboTempoAtual = comboTempoPadrao;
        comboEstado = ComboPicareta.none;

        picaretaGO[0].SetActive(false);
        picaretaGO[1].SetActive(true);
        abaixado = false;
        AnimaBool("Abaixar", abaixado);
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            AnimaBool("NoChao", true);
            CalculoMovimentoAng();

            //Pulo
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDir.y = puloForca;
            }
        }
        else
        {
            AnimaBool("NoChao", false);
        }

        moveDir.y -= gravidade * Time.deltaTime;
        controller.Move(moveDir);

        if (Input.GetKeyDown(KeyCode.C))
        {
            DefineAnimacaoAbaixar();
        }

        //Novo Armado Desarmado
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetLayerWeight(2, 0);
            picareta = !picareta;
            animator.SetTrigger("Armado");
        }

        if (picareta)
        {
            if (Input.GetKeyDown(KeyCode.P)) {

                if (comboEstado == ComboPicareta.golpe2)
                {
                    return;
                }

                comboEstado++;
                ativaTempoReset = true;
                comboTempoAtual = comboTempoPadrao;

                if (comboEstado == ComboPicareta.golpe1)
                {
                    AnimaBool("Golpe1", true);
                }
                if (comboEstado == ComboPicareta.golpe2)
                {
                    AnimaBool("Golpe2", true);
                }

                ResetCombo();

                if (moveX != 0 || moveZ != 0)
                {
                    animator.SetLayerWeight(4, 0);
                    animator.SetLayerWeight(3, 0.5f);
                }
                else if (moveX == 0 && moveZ == 0)
                {
                    animator.SetLayerWeight(3, 0);
                    animator.SetLayerWeight(4, 1);
                }
            }
        }
    }

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
            moveDir = Vector3.ClampMagnitude(moveDir,0.01f) * 2;
            controller.height = 1.26f;
            controller.center = new Vector3(0, 0.73f, 0);
        }
        else if(!Input.GetKey(KeyCode.LeftShift))
        {
            moveDir = Vector3.ClampMagnitude(moveDir, 0.01f) * 6;
            controller.height = 2.15f;
            controller.center = new Vector3(0, 1.12f, 0);
        }
    }

    private void Corrida()
    {
        if (moveZ > 0)
        {
            AnimaBool("MovimentoZ", true);
        }
        else
        {
            AnimaBool("MovimentoZ", false);
        }

        if (moveX != 0)
        {
            AnimaBool("MovimentoX", true);
        }
        else
        {
            AnimaBool("MovimentoX", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && !abaixado)
        {
            AnimaFloat("InputVertical", Input.GetAxis("Vertical") * 2.3f, 0.05f, Time.deltaTime * 10);
            AnimaFloat("InputHorizontal", Input.GetAxis("Horizontal") * 2.3f, 0.05f, Time.deltaTime * 10);
            moveDir = Vector3.ClampMagnitude(moveDir, 0.01f) * 11;
        }
    }

    private void AjustaPesoPicareta()
    {
        if (picareta)
        {
            animator.SetLayerWeight(2, 1);
        }
    }

    private void DefineArma_Armado()
    {
        if (picareta)
        {
            picaretaGO[0].SetActive(true);
            picaretaGO[1].SetActive(false);
        }
    }

    private void DefineArma_Desarmado()
    {
        if (!picareta)
        {
            picaretaGO[0].SetActive(false);
            picaretaGO[1].SetActive(true);
        }
    }

    private void ResetCombo()
    {
        print("Entrou mo ResetCombo");
        print("ativaTempoReset: " + ativaTempoReset);
        if (ativaTempoReset)
        {
            print("Entrou no ativaTempoReset");
            comboTempoAtual -= Time.deltaTime;

            print("comboTempoAtual: " + comboTempoAtual);
            if (comboTempoAtual <= 0)
            {
                print("Entrou mo comboTempoAtual");
                comboEstado = ComboPicareta.none;
                ativaTempoReset = false;
                comboTempoAtual = comboTempoPadrao;
            }
        }
    }

    private void FimAtaque()
    {
        AnimaBool("Golpe1", false);
    }

    private void FimCombo()
    {
        AnimaBool("Golpe2", false);
        AnimaBool("Golpe1", false);
    }
}