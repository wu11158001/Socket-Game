using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHash 
{
    public static int IsIdle = Animator.StringToHash("Idle");
    public static int IsDash = Animator.StringToHash("IsDash");
    public static int IsAttack = Animator.StringToHash("IsAttack");
    public static int IsJumpAttack = Animator.StringToHash("IsJumpAttack");
    public static int IsRun = Animator.StringToHash("IsRun");
    public static int Hurt_Tr = Animator.StringToHash("Hurt_Tr");
    public static int Win_Tr = Animator.StringToHash("Win_Tr");
    public static int Die_Tr = Animator.StringToHash("Die_Tr");
}
