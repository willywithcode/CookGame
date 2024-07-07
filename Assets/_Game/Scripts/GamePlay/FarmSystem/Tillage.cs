using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tillage : ACacheMonoBehaviour
{
    [SerializeField] private Transform pointContainer;
    [SerializeField] private Outline outline;
    public MeshRenderer meshRenderer;
    private bool isHavePlant = false;
    public TypeProcess typeProcess = TypeProcess.Plant;
    public ObjectStagePlant currentPlant;
    private Dictionary<TypeProcess, IPlantInteract> plantInteracts = new Dictionary<TypeProcess, IPlantInteract>()
    {
        { TypeProcess.Plant, new PlantInteract() },
        { TypeProcess.Water, new WaterInteract() },
        { TypeProcess.Harvest, new HavestInteract() }
    };
    public Transform PointContainer => pointContainer;

    public bool IsHavePlant
    {
        set => isHavePlant = value;
        get => isHavePlant;
    }
    private Plant plant;

    public Plant Plant
    {
        get => plant;
        set => plant = value;
    }

    public void Interact()
    {
        if(plant == null) return;
        if ( this.typeProcess != TypeProcess.Growing)
        {
            plantInteracts[this.typeProcess].Interact(plant, this);
        }
    }

    public void Interact(Plant plant)
    {
        this.plant = plant;
        this.typeProcess = TypeProcess.Plant;
        this.Interact();
    }
    public void ToggleOutLine(bool state) => outline.enabled = state;
}

public interface IPlantInteract
{
    public void Interact(Plant plant, Tillage tillage);
}
public class WaterInteract : IPlantInteract
{
    public void Interact(Plant plant, Tillage tillage)
    {
        Player player = GameManager.Instance.Player;
        player.StartWaterPlant();
        UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonInteractFarm(false);
        player.actionStateMachine.ChangeState(player.actionStateMachine.waterState);
        SoundManager.Instance.PlayAudio(SoundManager.DataSound.soundWater);
        player.actionStateMachine.waterState.AssignEvent(() =>
        {
            this.DoneWater(plant, tillage);
        });
        player.actionStateMachine.waterState.onNotEnd = () =>
        {
            SoundManager.Instance.StopAudio(SoundManager.DataSound.soundWater);
            player.DoneWaterPlant();
            tillage.typeProcess = TypeProcess.Growing;
            player.checkObjectSurround.GetCurrentTillage();
        };
        
    }

    public void DoneWater(Plant plant, Tillage tillage)
    {
        _ = plant.Growing( tillage);
    }
}
public class PlantInteract : IPlantInteract
{
    public void Interact(Plant plant,  Tillage tillage)
    {
        Player player = GameManager.Instance.Player;
        UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonInteractFarm(false);
        player.actionStateMachine.ChangeState(player.actionStateMachine.plantState);
        SoundManager.Instance.PlayAudio(SoundManager.DataSound.soundPlant);
        player.actionStateMachine.plantState.AssignEvent(() =>
        {
            this.DonePlant(plant, tillage);
        });
        player.actionStateMachine.plantState.onNotEnd = () =>
        {
            SoundManager.Instance.StopAudio(SoundManager.DataSound.soundPlant);
            player.checkObjectSurround.GetCurrentTillage();
        };

    }

    public void DonePlant(Plant plant,  Tillage tillage)
    {
        tillage.currentPlant = plant.seed.PoolObjectPlant.GetObject();
        tillage.currentPlant.TF.SetParent(tillage.PointContainer);
        tillage.currentPlant.TF.localPosition = Vector3.zero;
        tillage.currentPlant.TF.localScale = Vector3.one;
        tillage.IsHavePlant = true;
        tillage.typeProcess = TypeProcess.Water;
    }
}
public class HavestInteract : IPlantInteract
{
    public void Interact(Plant plant,  Tillage tillage)
    {
        Player player = GameManager.Instance.Player;
        UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonInteractFarm(false);
        player.actionStateMachine.ChangeState(player.actionStateMachine.havestState);
        SoundManager.Instance.PlayAudio(SoundManager.DataSound.soundHarvest);
        player.actionStateMachine.havestState.AssignEvent(() =>
        {
            this.DoneHavest(plant, tillage);
            UIManager.Instance.GetUI<UIGamePlay>().PopUpText("You received " + tillage.Plant.numberHarvest + " " + tillage.Plant.namePlant + "!");
            player.AddItemToInventory(tillage.Plant.numberHarvest, SaveGameManager.GetDataItem(tillage.Plant.namePlant).prefab);
        });
        player.actionStateMachine.plantState.onNotEnd = () =>
        {
            SoundManager.Instance.StopAudio(SoundManager.DataSound.soundHarvest);
            player.checkObjectSurround.GetCurrentTillage();
        };
        
    }

    public void DoneHavest(Plant plant,  Tillage tillage)
    {
        tillage.currentPlant.OnDespawn();
        tillage.typeProcess = TypeProcess.Plant;
        tillage.IsHavePlant = false;
        tillage.currentPlant = null;
    }
}

