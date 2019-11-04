select
  x.id,
  x.caption,
  x.appointment_id,
  coalesce(app.caption, '') as appointment_caption,
  coalesce(app.patient_card_id, uuid_in('00000000-0000-0000-0000-000000000000')) as patient_card_id,
  x.episode_id,
  coalesce(e.caption, '') as episode_caption,
  x.diagnosis_type_enum,
  (select caption from enum_record  where  enum_type = 'DiagnosisType'  and code = x.diagnosis_type_enum) as diagnosis_type,
  x.is_first_time_detected,
  x.diagnosis_source_type_enum,
  (select caption from enum_record  where  enum_type = 'DiagnosisSourceType'  and code = x.diagnosis_source_type_enum) as diagnosis_source_type,
  x.icpc2_id,
  coalesce(icpc.caption, '') as icpc2_caption,
  x.clinical_status_type_enum,
  (select caption from enum_record  where  enum_type = 'ClinicalStatusType'  and code = x.clinical_status_type_enum) as clinical_status_type,
  x.verification_status_type_enum,
  (select caption from enum_record  where  enum_type = 'VerificationStatusType'  and code = x.verification_status_type_enum) as verification_status_type,
  x.severity_state_degree_enum,
  (select caption from enum_record  where  enum_type = 'SeverityStateDegree'  and code = x.severity_state_degree_enum) as severity_state_degree,
  x.detection_date,
  x.confirmation_date,
  x.description,
  coalesce(doc.organization_id, uuid_in('00000000-0000-0000-0000-000000000000')) as organization_id
from mis_appointment_episode as x
  inner join mis_appointment as app on x.appointment_id = app.id and app.record_state <> 4
  left join org_employee as doc on app.doctor_id = doc.id and doc.record_state <> 4
  left join mis_episode as e on x.episode_id = e.id and e.record_state <> 4
  left join cdn_icpc2 as icpc on x.icpc2_id = icpc.id and icpc.record_state <> 4
where x.record_state <> 4