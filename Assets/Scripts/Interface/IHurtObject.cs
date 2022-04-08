using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHurtObject {
    //施加伤害接口
    void SendHurtRequest(int hurtValue, int ObjectID);
}
