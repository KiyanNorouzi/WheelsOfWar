using UnityEngine;
using System.Collections;

public class TestStaticClass : MonoBehaviour {
	
	[System.Serializable]
	public class HeliClass
	{
		[System.Serializable]
		public class HeliTypeClass
		{
			[System.Serializable]
			public class HeliIndexClass
			{
				public string title;
				public bool isSelect;
				public bool isActive;
				public bool isLock;
				public int Level;
			}
			
			public string title;
			public HeliIndexClass[] heliIndex;
			
		}
		public HeliTypeClass[] heliType;
	}




	public static  HeliClass heliClass;
	public HeliClass heliClassTemp=new HeliClass();

	
	public int heliTypeLength;
	public int heliIndexLength;
	public int heliWarLength;
	public int heliRescueLength;
	public int heliPartLength;
	public int heliItemLength;
	
	void Awake(){
		InitialHeliClass();
	}
	public void InitialHeliClass(){
		
		//initial HeliType
		heliClass=new HeliClass();
		heliClass.heliType=new HeliClass.HeliTypeClass[heliTypeLength];
		for(int i=0;i<heliTypeLength;i++) heliClass.heliType[i]=new HeliClass.HeliTypeClass();
		
		//initial HeliIndex
		heliClass.heliType[0].heliIndex=new HeliClass.HeliTypeClass.HeliIndexClass[heliIndexLength];
		for(int i=0;i<heliIndexLength;i++) heliClass.heliType[0].heliIndex[i]=new HeliClass.HeliTypeClass.HeliIndexClass();

        HeliTypeInitial(ref heliClass.heliType, heliClassTemp.heliType);

		//heliClassTemp.heliType=heliClass.heliType;

		heliClassTemp.heliType[0].title="Change";
		print(heliClass.heliType[0].title);
		
	}

    // se tashon yekari mikonand bishtar janbe amozeshi dare :)))
    // vaghti ref minevisi yani taghirat ro roye hamon variable asli eejad kon azash instance nasaz

    void HeliTypeInitial(ref HeliClass.HeliTypeClass[] h,out HeliClass.HeliTypeClass[] hnew)
    {
        hnew = new HeliClass.HeliTypeClass[h.Length]; 
        for (int i = 0; i < h.Length; i++)
        {
            hnew[i] = new HeliClass.HeliTypeClass();

            hnew[i].heliIndex = new HeliClass.HeliTypeClass.HeliIndexClass[hnew[i].heliIndex.Length];

            for (int j = 0; j < h[i].heliIndex.Length; j++)
            {
                hnew[i].heliIndex[j] = new HeliClass.HeliTypeClass.HeliIndexClass();

                hnew[i].heliIndex[j].isActive = h[i].heliIndex[j].isActive;
                hnew[i].heliIndex[j].isLock = h[i].heliIndex[j].isLock;
                hnew[i].heliIndex[j].isSelect = h[i].heliIndex[j].isSelect;
                hnew[i].heliIndex[j].Level = h[i].heliIndex[j].Level;
                hnew[i].heliIndex[j].title = h[i].heliIndex[j].title;
            }


            hnew[i].title =h[i].title;
        }
    }

    void HeliTypeInitial(ref HeliClass.HeliTypeClass[] h,HeliClass.HeliTypeClass[] hnew)
    {
        
        h = new HeliClass.HeliTypeClass[hnew.Length];
        for (int i = 0; i < h.Length; i++)
        {
            h[i] = new HeliClass.HeliTypeClass();

            h[i].heliIndex = new HeliClass.HeliTypeClass.HeliIndexClass[hnew[i].heliIndex.Length];

            for (int j = 0; j < h[i].heliIndex.Length; j++)
            {
                h[i].heliIndex[j] = new HeliClass.HeliTypeClass.HeliIndexClass();

                h[i].heliIndex[j].isActive = hnew[i].heliIndex[j].isActive;
                h[i].heliIndex[j].isLock = hnew[i].heliIndex[j].isLock;
                h[i].heliIndex[j].isSelect = hnew[i].heliIndex[j].isSelect;
                h[i].heliIndex[j].Level = hnew[i].heliIndex[j].Level;
                h[i].heliIndex[j].title = hnew[i].heliIndex[j].title;
            }


            h[i].title = hnew[i].title;
        }
    }

    void HeliTypeInitial(HeliClass.HeliTypeClass[] hnew, out HeliClass.HeliTypeClass[] h)
    {
        h = new HeliClass.HeliTypeClass[hnew.Length];
        for (int i = 0; i < h.Length; i++)
        {
            h[i] = new HeliClass.HeliTypeClass();

            h[i].heliIndex = new HeliClass.HeliTypeClass.HeliIndexClass[hnew[i].heliIndex.Length];

            for (int j = 0; j < hnew[i].heliIndex.Length; j++)
            {
                h[i].heliIndex[j] = new HeliClass.HeliTypeClass.HeliIndexClass();

                h[i].heliIndex[j].isActive = hnew[i].heliIndex[j].isActive;
                h[i].heliIndex[j].isLock = hnew[i].heliIndex[j].isLock;
                h[i].heliIndex[j].isSelect = hnew[i].heliIndex[j].isSelect;
                h[i].heliIndex[j].Level = hnew[i].heliIndex[j].Level;
                h[i].heliIndex[j].title = hnew[i].heliIndex[j].title;
            }

            h[i].title = hnew[i].title;
        }
    }
}
