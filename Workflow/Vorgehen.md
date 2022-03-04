# Procedure - Vorgehen

 1. create enumeration for the statuses - Enumeration für die Status anlegen
 2. create enumeration for the operations - Enumeration für die Operationen anlegen
 3. create domain class and implement IWorkflowParticipant - Domänenklasse anlegen und IWorkflowParticipant implementieren
 4. create workflow class for domain class - Workflow-Klasse für Domänenklasse anlegen
    - implement abstract method GetOperations - abstrakte Methode GetOperations implementieren
    - implement abstract method GetNextState - abstrakte Methode GetNextState implementieren
    - can implement virtual method GetRequiredProperties - virtuelle Methode GetRequiredProperties kann implementiert werden
    - can implement virtual method GetActionToInvoke - virtuelle Methode GetActionToInvoke kann implementiert werden