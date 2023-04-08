using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public List<GameObject> Cars = new List<GameObject>();
    public List<GameObject> CarSpawnPoints = new List<GameObject>();
    public List<Material> CarBodyMaterials = new List<Material>();
    public float carSpeed;
    public Transform LeftLineXaxis,RightLineXaxis;
     
     
    private void Update()
    {
        for (int i = 0; i < Cars.Count-1; i++)
        {
            if (Cars[i].transform.position.x > LeftLineXaxis.position.x)
            {
                Cars[2].transform.Translate(-carSpeed * Time.deltaTime, 0, 0);
                Cars[3].transform.Translate(-carSpeed * Time.deltaTime, 0, 0);

            }
           
            if(Cars[i].transform.position.x < RightLineXaxis.position.x)
            {
                Cars[0].transform.Translate(carSpeed * Time.deltaTime, 0, 0);
                Cars[1].transform.Translate(carSpeed * Time.deltaTime, 0, 0);
            }

            if (LineCheck.›nstance_LineCheck.CarCollisionCheck == true)
            {
                //carSpeed = Random.Range(2,6);
                int mat1 = Random.Range(0, 14);
                int mat2 = Random.Range(0, 14);
                int mat3 = Random.Range(0, 14);
                int mat4 = Random.Range(0, 14);
                Cars[0].transform.position = CarSpawnPoints[0].transform.position;
                Cars[1].transform.position = CarSpawnPoints[1].transform.position;
                Cars[2].transform.position = CarSpawnPoints[2].transform.position;
                Cars[3].transform.position = CarSpawnPoints[3].transform.position;
                CarsGetChild(0, 0, CarBodyMaterials[mat1], CarBodyMaterials[mat2], CarBodyMaterials[mat3], CarBodyMaterials[mat4]);
                CarsGetChild(0, 1, CarBodyMaterials[mat1], CarBodyMaterials[mat2], CarBodyMaterials[mat3], CarBodyMaterials[mat4]);
                LineCheck.›nstance_LineCheck.CarCollisionCheck = false;
            }
            
        }
    }


    void CarsGetChild(int child›ndex0,int child›ndex1, Material mat1, Material mat2, Material mat3, Material mat4)
    {

        Cars[0].transform.GetChild(child›ndex0).transform.GetChild(child›ndex0).gameObject.GetComponent<MeshRenderer>().material = mat1;
        Cars[1].transform.GetChild(child›ndex0).transform.GetChild(child›ndex0).gameObject.GetComponent<MeshRenderer>().material = mat2;
        Cars[2].transform.GetChild(child›ndex0).transform.GetChild(child›ndex0).gameObject.GetComponent<MeshRenderer>().material = mat3;
        Cars[3].transform.GetChild(child›ndex0).transform.GetChild(child›ndex0).gameObject.GetComponent<MeshRenderer>().material = mat4;

        Cars[0].transform.GetChild(child›ndex0).transform.GetChild(child›ndex1).gameObject.GetComponent<MeshRenderer>().material = mat1;
        Cars[1].transform.GetChild(child›ndex0).transform.GetChild(child›ndex1).gameObject.GetComponent<MeshRenderer>().material = mat2;
        Cars[2].transform.GetChild(child›ndex0).transform.GetChild(child›ndex1).gameObject.GetComponent<MeshRenderer>().material = mat3;
        Cars[3].transform.GetChild(child›ndex0).transform.GetChild(child›ndex1).gameObject.GetComponent<MeshRenderer>().material = mat4;
    }

}
