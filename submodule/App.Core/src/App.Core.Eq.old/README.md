#Модуль EQ

###	При створенні розкладу на співробітника (відбувається вибір людини П.І.Б, та посади, яку вона обіймає).
В таблицю «Resources» записуються :
*	 Поле «EnityRecordId»  =  «OrgEmployeeId» (EnityRecordId  відповідає картці співробітника табл. «OrgEmployees»)
*	Поле «OrgEmployeeId»  =  «OrgUnitPositionEmployeeId» (фактично неправильно найменоване поле, OrgEmployeeId відповідає значенню «Id» таблиці «OrgUnitPositionEmployee» і слугає для визначення однозначності посади конкретно обраного співробітника) 
