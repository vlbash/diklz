--SELECT
--	med."id",
--	med.caption,
--	med.medicine_name,
--	med.form_name,
--	med.dose_in_unit,
--	med.number_of_units,
--	med.register_number,
--	med.atc_code,
--	med.producer_name,
--    med.producer_country,
--	med.supplier_name,
--    med.supplier_country,
--	med.supplier_address,
--	med.notes,
--    med.lims_rp_id,
--	med.medicine_name_eng,
--    med.application_id,
--    med.is_from_license,
--    imla.org_unit_id
--FROM
--	iml_medicines AS med
--left join iml_applications as imla on imla.id = med.application_id

SELECT
	med."id",
	coalesce(med.caption,'') as caption,
	coalesce(med.medicine_name,'') as medicine_name,
	coalesce(med.form_name,'') as form_name,
	coalesce(med.dose_in_unit,'') as dose_in_unit,
	coalesce(med.number_of_units,'') as number_of_units,
	coalesce(med.register_number,'') as register_number,
	coalesce(med.atc_code,'') as atc_code,
	coalesce(med.producer_name,'') as producer_name,
    coalesce(med.producer_country,'') as producer_country,
	coalesce(med.supplier_name,'') as supplier_name,
    coalesce(med.supplier_country,'') as supplier_country,
	coalesce(med.supplier_address,'') as supplier_address,
	coalesce(med.notes,'') as notes,
    med.lims_rp_id,
	coalesce(med.medicine_name_eng,'') as medicine_name_eng,
    med.application_id,
    med.is_from_license,
    imla.org_unit_id
FROM
	iml_medicines AS med
left join iml_applications as imla on imla.id = med.application_id
