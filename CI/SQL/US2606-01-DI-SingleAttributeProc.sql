-- get the list of attribute types that need to be unique
declare @single_attr_tab TABLE(attr_type_id int, attr_id int, unique_attribute_name varchar(max))

insert into @single_attr_tab select attr_t.Attribute_Type_ID, Attribute_ID, attr_t.Attribute_Type from Attribute_Types attr_t inner join Attributes attr on attr_t.Attribute_Type_ID = attr.Attribute_Type_ID where attr_t.Prevent_Multiple_Selection=1

select * from (select attr_type_id, contact_id, count(contact_id) attr_count from Contact_Attributes ca inner join @single_attr_tab sa on ca.Attribute_ID = sa.attr_id where ca.End_Date is null group by attr_type_id, contact_id) as x where attr_count > 1 order by attr_type_id

select attr_type_id, contact_id, count(contact_id) attr_count from Contact_Attributes ca inner join @single_attr_tab sa on ca.Attribute_ID = sa.attr_id where ca.End_Date is null group by attr_type_id, contact_id having count(contact_id) > 1 order by attr_type_id
