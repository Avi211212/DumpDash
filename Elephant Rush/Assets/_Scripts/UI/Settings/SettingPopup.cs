﻿using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Facebook.Unity;
using UnityEditor.Rendering;

public class SettingPopup : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider masterSFXSlider;

    public LoadoutState loadoutState;
    public DataDeleteConfirmation confirmationPopup;

    protected float m_MasterVolume;
    protected float m_MusicVolume;
    protected float m_MasterSFXVolume;

    protected const float k_MinVolume = -20f;
    protected const string k_MasterVolumeFloatName = "MasterVolume";
    protected const string k_MusicVolumeFloatName = "MusicVolume";
    protected const string k_MasterSFXVolumeFloatName = "MasterSFXVolume";

    public void Open()
    {
        loadoutState.character.ShouldRotate(false);
        gameObject.SetActive(true);
        UpdateUI();
    }

    public void Close()
    {
        loadoutState.character.ShouldRotate(true);
        PlayerData.instance.Save();
        gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        mixer.GetFloat(k_MasterVolumeFloatName, out m_MasterVolume);
        mixer.GetFloat(k_MusicVolumeFloatName, out m_MusicVolume);
        mixer.GetFloat(k_MasterSFXVolumeFloatName, out m_MasterSFXVolume);

        masterSlider.value = 1.0f - (m_MasterVolume / k_MinVolume);
        musicSlider.value = 1.0f - (m_MusicVolume / k_MinVolume);
        masterSFXSlider.value = 1.0f - (m_MasterSFXVolume / k_MinVolume);
    }

    public void DeleteData()
    {
        confirmationPopup.Open(loadoutState);
    }

    public void MasterVolumeChangeValue(float value)
    {
        if(value == 0)
        {
            mixer.SetFloat(k_MasterVolumeFloatName, -80f);
            PlayerData.instance.masterVolume = m_MasterVolume;
        }
        else
        {
            m_MasterVolume = k_MinVolume * (1.0f - value);
            mixer.SetFloat(k_MasterVolumeFloatName, m_MasterVolume);
            PlayerData.instance.masterVolume = m_MasterVolume;
        }
    }

    public void MusicVolumeChangeValue(float value)
    {
        if(value == 0)
        {
            mixer.SetFloat(k_MusicVolumeFloatName, -80f);
            PlayerData.instance.musicVolume = m_MusicVolume;
        }
        else
        {
            m_MusicVolume = k_MinVolume * (1.0f - value);
            mixer.SetFloat(k_MusicVolumeFloatName, m_MusicVolume);
            PlayerData.instance.musicVolume = m_MusicVolume;
        }
    }

    public void MasterSFXVolumeChangeValue(float value)
    {
        if(value == 0)
        {
            mixer.SetFloat(k_MasterSFXVolumeFloatName, -80f);
            PlayerData.instance.masterSFXVolume = m_MasterSFXVolume;
        }
        else
        {
            m_MasterSFXVolume = k_MinVolume * (1.0f - value);
            mixer.SetFloat(k_MasterSFXVolumeFloatName, m_MasterSFXVolume);
            PlayerData.instance.masterSFXVolume = m_MasterSFXVolume;
        }
    }
}
