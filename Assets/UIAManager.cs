using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAManager : MonoBehaviour
{
    [SerializeField] UIAPanel uiaPanel;

    bool isEgressComplete = false;
    bool isEva1 = true;
    public AudioSource sound;

    float temp_val1 = 0f;
    float temp_val2 = 0f;

    [ContextMenu("func SetUIAPanel")]
    public void SetUIAPanel()
    {
        uiaPanel.SetPanelPosition();

        if (!isEgressComplete)
        {
            uiaPanel.SetText("Connect UIA and DCU umbilical. Then say \"Start Egress\"", "center");
        }
        else
        {
            uiaPanel.SetText("Connect UIA and DCU umbilical. Then say \"Start Ingress\"", "center");
        }
    }

    private void Start()
    {
        EventBus.Subscribe<StartEgress>(_ => StartEgress());
        EventBus.Subscribe<StartIngress>(_ => StartIngress());

        EventBus.Subscribe<UIAChanged>(e =>
        {
            isEva1 = AstronautInstance.User.id == 0;
            if (!isEgressComplete)
            {
                UpdateEgress(e.data);
                ShowEgress();
            }
            else
            {
                UpdateIngress();
                ShowIngress();
            }
        });
    }

    public int EgressStep = 0;
    public void StartEgress()
    {
        EgressStep = 1;
        ShowEgress();
    }

    void UpdateEgress(UiDetails data)
    {
        // check if we go to the next step
        switch (EgressStep)
        {
            case 1:
                // EV-1, EV-2 PWR – ON
                if (isEva1 && data.eva1_power)
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && data.eva2_power)
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 2:
                // BATT – UMB
                if (isEva1 && (AstronautInstance.User.dcu.eva1.batt == false))
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && (AstronautInstance.User.dcu.eva2.batt == false))
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 3:
                // DEPRESS PUMP PWR – ON
                if (data.depress)
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 4:
                // OXYGEN O2 VENT – OPEN
                if (data.oxy_vent)
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 5:
                // Wait until both Primary and Secondary OXY tanks are < 10psi
                if (Mathf.Approximately(temp_val1, 0f))
                {
                    temp_val1 = (float)AstronautInstance.User.VitalsData.oxy_pri_pressure;
                    temp_val2 = (float)AstronautInstance.User.VitalsData.oxy_sec_pressure;
                }
                float psi1 = (float)AstronautInstance.User.VitalsData.oxy_pri_pressure;
                float psi2 = (float)AstronautInstance.User.VitalsData.oxy_sec_pressure;

                float p1 = Mathf.Max(0f, (temp_val1 - psi1 - 10f) / (temp_val1 - 10f));
                float p2 = Mathf.Max(0f, (temp_val2 - psi2 - 10f) / (temp_val2 - 10f));

                float percentage = (p1 + p2) * 100 / 2;

                uiaPanel.ProgressBar.Update_Progress_bar(100 - Mathf.RoundToInt(percentage), 100);

                if (psi1 < 10f && psi2 < 10f)
                {
                    EgressStep++;
                    temp_val1 = 0f;
                    temp_val2 = 0f;
                    sound.Play();
                }
                break;
            case 6:
                // OXYGEN O2 VENT – CLOSE
                if (data.oxy_vent == false)
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 7:
                // OXY – PRI
                if (isEva1 && (AstronautInstance.User.dcu.eva1.oxy == true))
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && (AstronautInstance.User.dcu.eva2.oxy == true))
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 8:
                // OXYGEN EMU-1, EMU-2 – OPEN
                if (isEva1 && data.eva1_oxy)
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && data.eva2_oxy)
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 9:
                // Wait until EV1 and EV2 Primary O2 tanks > 3000 psi
                float psi1_ = (float)AstronautInstance.User.VitalsData.oxy_pri_pressure;
                float psi2_ = (float)AstronautInstance.User.FellowAstronautsData.vitals.oxy_pri_pressure;

                float p1_ = Mathf.Min(psi1_ / 3000f, 1f);
                float p2_ = Mathf.Min(psi2_ / 3000f, 1f);

                float percentage_ = (p1_ + p2_) * 100 / 2;

                uiaPanel.ProgressBar.Update_Progress_bar(Mathf.RoundToInt(percentage_), 100);

                if (psi1_ > 3000f && psi2_ > 3000f)
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 10:
                // OXYGEN EMU - 1, EMU - 2 – CLOSE
                if (isEva1 && (data.eva1_oxy == false))
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && (data.eva2_oxy == false))
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 11:
                // OXY – SEC
                if (isEva1 && (AstronautInstance.User.dcu.eva1.oxy == false))
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && (AstronautInstance.User.dcu.eva2.oxy == false))
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 12:
                // OXYGEN EMU-1, EMU-2 – OPEN
                if (isEva1 && data.eva1_oxy)
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && data.eva2_oxy)
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 13:
                // Wait until EV1 and EV2 Secondary O2 tanks > 3000 psi
                float psi1__ = (float)AstronautInstance.User.VitalsData.oxy_sec_pressure;
                float psi2__ = (float)AstronautInstance.User.FellowAstronautsData.vitals.oxy_sec_pressure;

                float p1__ = Mathf.Min(psi1__ / 3000f, 1f);
                float p2__ = Mathf.Min(psi2__ / 3000f, 1f);

                float percentage__ = (p1__ + p2__) * 100 / 2;

                uiaPanel.ProgressBar.Update_Progress_bar(Mathf.RoundToInt(percentage__), 100);

                if (psi1__ > 3000f && psi2__ > 3000f)
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 14:
                // OXYGEN EMU - 1, EMU - 2 – CLOSE
                if (isEva1 && (data.eva1_oxy == false))
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && (data.eva2_oxy == false))
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 15:
                // OXY – PRI
                if (isEva1 && (AstronautInstance.User.dcu.eva1.oxy == true))
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && (AstronautInstance.User.dcu.eva2.oxy == true))
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 16:
                // PUMP – OPEN
                if (isEva1 && (AstronautInstance.User.dcu.eva1.pump == true))
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && (AstronautInstance.User.dcu.eva2.pump == true))
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 17:
                // EV-1, EV-2 WASTE WATER – OPEN
                if (isEva1 && data.eva1_water_waste)
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && data.eva2_water_waste)
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            case 18:
                // Wait until EV1 and EV2 Secondary O2 tanks > 3000 psi
                //      if (Mathf.Approximately(temp_val1, 0f))
                //      {
                //          temp_val1 = (float)AstronautInstance.User.VitalsData;
                //          temp_val2 = (float)AstronautInstance.User.VitalsData.oxy_sec_pressure;
                //      }
                //      float psi1 = (float)AstronautInstance.User.VitalsData.oxy_pri_pressure;
                //      float psi2 = (float)AstronautInstance.User.VitalsData.oxy_sec_pressure;
                //      
                //      float p1 = Mathf.Max(0f, (temp_val1 - psi1 - 10f) / (temp_val1 - 10f));
                //      float p2 = Mathf.Max(0f, (temp_val2 - psi2 - 10f) / (temp_val2 - 10f));
                //      
                //      float percentage = (p1 + p2) * 100 / 2;
                //      uiaPanel.ProgressBar.Update_Progress_bar(Mathf.RoundToInt(percentage__), 100);
                //      
                //      if (psi1__ > 3000f && psi2__ > 3000f)
                //      {
                //          EgressStep++;
                //          temp_val1 = 0f;
                //          temp_val2 = 0f;
                //          sound.Play();
                //      }
                //      break;
                break;
            case 19:
                // EV-1, EV-2 WASTE WATER – CLOSE
                if (isEva1 && !data.eva1_water_waste)
                {
                    EgressStep++;
                    sound.Play();
                }
                else if (!isEva1 && !data.eva2_water_waste)
                {
                    EgressStep++;
                    sound.Play();
                }
                break;
            default:
                break;
        }
    }
    void ShowEgress()
    {
        // show the current step
        switch (EgressStep)
        {
            case 1:
                if (isEva1)
                {
                    uiaPanel.SetText("Set UIA EV-1 PWR to ON", "center");
                    uiaPanel.SetButton(9, true);
                }
                else
                {
                    uiaPanel.SetText("Set UIA EV-2 PWR to ON", "center");
                    uiaPanel.SetButton(8, true);
                }
                break;
            case 2:
                uiaPanel.SetText("Set DCU BATT to UMB", "center");
                uiaPanel.HideAllButtons();
                break;
            case 3:
                uiaPanel.SetText("Set UIA DEPRESS PUMP PWR to ON", "center");
                uiaPanel.SetButton(7, true);
                break;
            case 4:
                uiaPanel.SetText("Set UIA OXYGEN O2 VENT to OPEN", "center");
                uiaPanel.SetButton(6, true);
                break;
            case 5:
                uiaPanel.SetText("Wait until both Primary and Secondary OXY tanks are < 10psi", "lower");
                uiaPanel.HideAllButtons();
                break;
            case 6:
                uiaPanel.SetText("Set UIA OXYGEN O2 VENT to CLOSE", "center");
                uiaPanel.SetButton(6, false);
                break;
            case 7:
                uiaPanel.SetText("Set DCU OXYGEN to PRIMARY TANK", "center");
                uiaPanel.HideAllButtons();
                break;
            case 8:
                if (isEva1)
                {
                    uiaPanel.SetText("Set UIA OXYGEN EMU-1 to OPEN", "center");
                    uiaPanel.SetButton(4, true);
                }
                else
                {
                    uiaPanel.SetText("Set UIA OXYGEN EMU-2 to OPEN", "center");
                    uiaPanel.SetButton(5, true);
                }
                break;
            case 9:
                uiaPanel.SetText("Wait until both EV1 and EV2 Primary O2 tanks > 3000psi", "lower");
                uiaPanel.HideAllButtons();
                break;
            case 10:
                if (isEva1)
                {
                    uiaPanel.SetText("Set UIA OXYGEN EMU-1 to CLOSE", "center");
                    uiaPanel.SetButton(4, false);
                }
                else
                {
                    uiaPanel.SetText("Set UIA OXYGEN EMU-2 to CLOSE", "center");
                    uiaPanel.SetButton(5, false);
                }
                break;
            case 11:
                uiaPanel.SetText("Set DCU OXYGEN to SECONDARY TANK", "center");
                uiaPanel.HideAllButtons();
                break;
            case 12:
                if (isEva1)
                {
                    uiaPanel.SetText("Set UIA OXYGEN EMU-1 to OPEN", "center");
                    uiaPanel.SetButton(4, true);
                }
                else
                {
                    uiaPanel.SetText("Set UIA OXYGEN EMU-2 to OPEN", "center");
                    uiaPanel.SetButton(5, true);
                }
                break;
            case 13:
                uiaPanel.SetText("Wait until both EV1 and EV2 Secondary O2 tanks > 3000psi", "lower");
                uiaPanel.HideAllButtons();
                break;
            case 14:
                if (isEva1)
                {
                    uiaPanel.SetText("Set UIA OXYGEN EMU-1 to CLOSE", "center");
                    uiaPanel.SetButton(4, false);
                }
                else
                {
                    uiaPanel.SetText("Set UIA OXYGEN EMU-2 to CLOSE", "center");
                    uiaPanel.SetButton(5, false);
                }
                break;
            case 15:
                uiaPanel.SetText("Set DCU OXYGEN to PRIMARY TANK", "center");
                uiaPanel.HideAllButtons();
                break;
            case 16:
                uiaPanel.SetText("Set DCU PUMP to OPEN", "center");
                uiaPanel.HideAllButtons();
                break;
            case 17:
                if (isEva1)
                {
                    uiaPanel.SetText("Set UIA EV-1 WASTE WATER to OPEN", "center");
                    uiaPanel.SetButton(1, true);
                }
                else
                {
                    uiaPanel.SetText("Set UIA EV-2 WASTE WATER to OPEN", "center");
                    uiaPanel.SetButton(3, true);
                }
                break;
            case 18:
                uiaPanel.SetText("Wait until water EV1 and EV2 Coolant tank is < 5%", "lower");
                uiaPanel.HideAllButtons();
                break;
            case 19:
                if (isEva1)
                {
                    uiaPanel.SetText("Set UIA EV-1 WASTE WATER to CLOSE", "center");
                    uiaPanel.SetButton(1, false);
                }
                else
                {
                    uiaPanel.SetText("Set UIA EV-2 WASTE WATER to CLOSE", "center");
                    uiaPanel.SetButton(3, false);
                }
                break;
            default:
                break;
        }
    }



    public int IngressStep = 0;
    public void StartIngress()
    {
        IngressStep = 1;
        UpdateIngress();
    }
    void UpdateIngress()
    {
        // check if we can go to next step
        switch (IngressStep)
        {
            case 1:
                uiaPanel.SetText("", "center");
                break;
            default:
                break;
        }
    }
    void ShowIngress()
    {
        // show the current step
        switch (IngressStep)
        {
            case 1:
                uiaPanel.SetText("", "center");
                break;
            default:
                break;
        }
    }
}
